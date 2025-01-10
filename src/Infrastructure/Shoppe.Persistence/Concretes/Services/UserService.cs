using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.UserProfilePictureFileRepos;
using Shoppe.Application.Abstractions.Repositories.UserProfilePictureRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Auth;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.Services.Token;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Role;
using Shoppe.Application.DTOs.Token;
using Shoppe.Application.DTOs.User;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Constants;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfilePictureFileReadRepository _userProfilePictureFileReadRepository;
        private readonly IUserProfilePictureFileWriteRepository _userProfilePictureFileWriteRepository;
        private readonly IPaginationService _paginationService;
        private readonly IStorageService _storage;
        private readonly IJwtSession _jwtSession;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;


        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IStorageService storage,
            IUserProfilePictureFileReadRepository userProfilePictureFileReadRepository,
            IUserProfilePictureFileWriteRepository userProfilePictureFileWriteRepository,
            IPaginationService paginationService,
            IJwtSession jwtSession,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IAuthService authService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _storage = storage;
            _userProfilePictureFileReadRepository = userProfilePictureFileReadRepository;
            _userProfilePictureFileWriteRepository = userProfilePictureFileWriteRepository;
            _paginationService = paginationService;
            _jwtSession = jwtSession;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _authService = authService;
        }

        public async Task AssignRolesAsync(string userId, List<string> roles, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateSuperAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            var userRoles = await _userManager.GetRolesAsync(user);
            // var rolesToDelete = userRoles.Except(roles).ToList();

            // var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToDelete);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

            CheckDeleteSucceeded(removeResult);

            if (roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        throw new ValidationException($"Role '{role}' does not exist.");
                    }
                }

                var updateResult = await _userManager.AddToRolesAsync(user, roles);

                CheckUpdateSucceeded(updateResult);
            }

            scope.Complete();
        }

        public async Task ChangeProfilePictureAsync(string userId, string? newImageId, IFormFile? newImageFile, CancellationToken cancellationToken)
        {
            ValidateAdminOrUserAccess(userId);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            var newProfilePicture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.Id.ToString() == newImageId);

            if (newProfilePicture == null && newImageFile != null)
            {
                var (path, fileName) = await _storage.UploadAsync(UserConst.ImagesFolder, newImageFile);

                newProfilePicture = new UserProfileImageFile
                {
                    FileName = fileName,
                    PathName = fileName,
                    Storage = _storage.StorageName,
                };
            }

            if (newProfilePicture == null) throw new EntityNotFoundException(nameof(newProfilePicture));

            var existingProfilePicture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.IsMain);

            if (existingProfilePicture == newProfilePicture) return;

            if (existingProfilePicture != null)
            {
                existingProfilePicture.IsMain = false;
            }

            newProfilePicture.IsMain = true;
            var result = await _userManager.UpdateAsync(user);

            CheckUpdateSucceeded(result);

            scope.Complete();
        }

        public async Task ToggleUserAsync(string userId, CancellationToken cancellationToken)
        {
            ValidateAdminOrUserAccess(userId);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            user.IsActive = !user.IsActive;
            user.DeactivatedAt = user.IsActive ? null : DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            CheckUpdateSucceeded(result);

            scope.Complete();
        }

        public async Task DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            ValidateAdminOrUserAccess(userId);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users  
               .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            var result = await _userManager.DeleteAsync(user);

            CheckDeleteSucceeded(result);

            foreach (var picture in user.ProfilePictureFiles)
            {
                if (_userProfilePictureFileWriteRepository.Delete(picture))
                {
                    await _storage.DeleteAsync(picture.PathName, picture.FileName);
                }
            }

            scope.Complete();
        }

        public async Task<GetAllUsersDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var userQuery = _userManager.Users.AsNoTracking();

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, userQuery, cancellationToken);

            var users = await paginationResult.PaginatedQuery.ToListAsync(cancellationToken);

            return new GetAllUsersDTO
            {
                TotalPages = paginationResult.TotalPages,
                TotalItems = paginationResult.TotalItems,
                PageSize = pageSize,
                Page = page,
                Users = users.Select(user => user.ToGetUserDTO()).ToList(),
            };
        }

        public async Task<GetUserDTO> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users          
               .AsNoTracking()
               .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));


            return user.ToGetUserDTO();
        }

        public async Task<List<GetImageFileDTO>> GetImagesAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users         
                         .AsNoTracking()
                         .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            return user.ProfilePictureFiles.Where(p => !p.IsMain).Select(p => p.ToGetImageFileDTO()).ToList();
        }

        
          public async Task<List<GetRoleDTO>> GetRolesAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
             .AsNoTracking()
             .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = await _roleManager.Roles
                .Where(role => userRoles.Contains(role.Name ?? string.Empty)).AsNoTracking()
                .ToListAsync(cancellationToken);

            return roles.Select(role => new GetRoleDTO
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description!,
                CreatedAt = role.CreatedAt,
            }).ToList();
        }
         
      
        public async Task RemovePictureAsync(string userId, string pictureId, CancellationToken cancellationToken)
        {
            ValidateAdminOrUserAccess(userId);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            var picture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.Id.ToString() == pictureId);

            if (picture == null) throw new EntityNotFoundException(nameof(picture));

            if (user.ProfilePictureFiles.Remove(picture))
            {
                if (_userProfilePictureFileWriteRepository.Delete(picture))
                {
                    var result = await _userManager.UpdateAsync(user);

                    CheckUpdateSucceeded(result);

                    await _storage.DeleteAsync(picture.PathName, picture.FileName);
                }
            }

            scope.Complete();
        }

        public async Task<TokenDTO?> UpdateAsync(UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            ValidateAdminOrUserAccess(updateUserDTO.UserId);

            bool isUser = _jwtSession.GetUserId() == updateUserDTO.UserId;

            TokenDTO? token = null;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users         
             .FirstOrDefaultAsync(u => u.Id == updateUserDTO.UserId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            if (updateUserDTO.FirstName != null && user.FirstName != updateUserDTO.FirstName)
                user.FirstName = updateUserDTO.FirstName;

            if (updateUserDTO.LastName != null && user.LastName != updateUserDTO.LastName)
                user.LastName = updateUserDTO.LastName;

            if (updateUserDTO.UserName != null && user.UserName != updateUserDTO.UserName)
            {
                var userByName = await _userManager.FindByNameAsync(updateUserDTO.UserName);
                if (userByName != null) throw new ValidationException("Username is already defined, choose another name");
                user.UserName = updateUserDTO.UserName;
            }

            if (updateUserDTO.Email != null && user.Email != updateUserDTO.Email)
            {
                var userByEmail = await _userManager.FindByEmailAsync(updateUserDTO.Email);
                if (userByEmail != null) throw new ValidationException("Email is already defined, choose another email");
                user.Email = updateUserDTO.Email;
            }

            if (updateUserDTO.Phone != null && user.PhoneNumber != updateUserDTO.Phone)
            {
                string phonePattern = @"^\+?[1-9]\d{1,14}$";

                if (Regex.IsMatch(updateUserDTO.Phone, phonePattern))
                {
                    var userByPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == updateUserDTO.Phone, cancellationToken);

                    if (userByPhone != null) throw new ValidationException("Phone is already defined, choose another phone");

                    user.PhoneNumber = updateUserDTO.Phone;
                }
                else
                {
                    throw new ValidationException("Invalid phone number format.");
                }
            }

            if (updateUserDTO.NewProfilePictureFile != null && updateUserDTO.NewProfilePictureFile.Length > 0)
            {
                var (path, fileName) = await _storage.UploadAsync(UserConst.ImagesFolder, updateUserDTO.NewProfilePictureFile);
                var existingProfilePicture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.IsMain);

                if (existingProfilePicture != null)
                {
                    existingProfilePicture.IsMain = false;
                }
                user.ProfilePictureFiles.Add(new UserProfileImageFile
                {
                    FileName = fileName,
                    PathName = path,
                    Storage = _storage.StorageName,
                    IsMain = true
                });
            }
            else if (updateUserDTO.AlreadyExistingImageId != null)
            {
                var newProfilePicture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.Id.ToString() == updateUserDTO.AlreadyExistingImageId);
                if (newProfilePicture == null) throw new EntityNotFoundException(nameof(newProfilePicture));

                var existingProfilePicture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.IsMain);
                if (existingProfilePicture != null)
                {
                    existingProfilePicture.IsMain = false;

                    // for now double save
                    CheckUpdateSucceeded(await _userManager.UpdateAsync(user));
                }

                newProfilePicture.IsMain = true;
            }

            if (!string.IsNullOrWhiteSpace(updateUserDTO.NewPassword) && !string.IsNullOrWhiteSpace(updateUserDTO.CurrentPassword))
            {
                if(!await _userManager.CheckPasswordAsync(user, updateUserDTO.CurrentPassword))
                {
                    throw new ValidationException("Current password is invalid");
                }

                if (updateUserDTO.ConfirmNewPassword != updateUserDTO.NewPassword)
                {
                    throw new ValidationException("Passwords do not match");
                }

               var passwordUpdateResult = await _userManager.ChangePasswordAsync(user, updateUserDTO.CurrentPassword, updateUserDTO.NewPassword);

                CheckPasswordUpdateSucceeded(passwordUpdateResult);
                
            }

            if (isUser)
            {
                token = await _tokenService.CreateAccessTokenAsync(user);
                user.RefreshToken = token.RefreshToken;
                await _authService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpiresAt);
            }

            var result = await _userManager.UpdateAsync(user);
            CheckUpdateSucceeded(result);

            scope.Complete();

            return token;
        }

        private void ValidateAdminOrUserAccess(string userId)
        {
            if (!_jwtSession.IsAdmin() && _jwtSession.GetUserId() != userId)
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        private async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) throw new EntityNotFoundException(nameof(ApplicationUser));

            return user;
        }

        private static void CheckUpdateSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new UpdateNotSucceedException("User update operation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        private static void CheckPasswordUpdateSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new UpdateNotSucceedException("User password update operation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        private static void CheckDeleteSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new DeleteNotSucceedException("Delete operation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}

/*
   public async Task<List<GetRoleDTO>> GetRolesAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(user));

            return user.UserRoles.Select(ur => new GetRoleDTO
            {
                Id = ur.Role.Id,
                Name = ur.Role.Name!,
                Description = ur.Role.Description!,
                CreatedAt = ur.Role.CreatedAt,
            }).ToList();
        }

 */