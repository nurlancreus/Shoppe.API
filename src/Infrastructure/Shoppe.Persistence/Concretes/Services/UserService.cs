using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.UserProfilePictureFileRepos;
using Shoppe.Application.Abstractions.Repositories.UserProfilePictureRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Role;
using Shoppe.Application.DTOs.User;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserProfilePictureFileReadRepository _userProfilePictureFileReadRepository;
        private readonly IUserProfilePictureFileWriteRepository _userProfilePictureFileWriteRepository;
        private readonly IPaginationService _paginationService;
        private readonly IStorage _storage;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IStorage storage, IUserProfilePictureFileReadRepository userProfilePictureFileReadRepository, IUserProfilePictureFileWriteRepository userProfilePictureFileWriteRepository, IPaginationService paginationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _storage = storage;
            _userProfilePictureFileReadRepository = userProfilePictureFileReadRepository;
            _userProfilePictureFileWriteRepository = userProfilePictureFileWriteRepository;
            _paginationService = paginationService;
        }

        public async Task AssignRolesAsync(string userId, List<string> roles, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userRolesSet = new HashSet<string>(userRoles); // For faster lookups

            // Determine roles to delete
            var rolesToDelete = userRolesSet.Where(r => !roles.Contains(r)).ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToDelete);

            // Add roles
            foreach (var roleName in roles)
            {
                // Await the async operation to get the role
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role == null)
                {
                    throw new EntityNotFoundException(nameof(role));
                }

                if (!userRolesSet.Contains(roleName)) // Check if role is already assigned
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            await _userManager.UpdateAsync(user);
        }


        public async Task ChangeProfilePictureAsync(string userId, string newPictureId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.ProfilePictureFiles).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            if (user.ProfilePictureFiles.Count == 0) return;

            var existingProfilePicture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.IsMain);
            var newProfilePicture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.Id.ToString() == newPictureId);

            if (newProfilePicture == null)
            {
                throw new EntityNotFoundException(nameof(newProfilePicture));
            }

            if (existingProfilePicture == newProfilePicture) return;

            if (existingProfilePicture != null)
            {
                existingProfilePicture.IsMain = false;
            }

            newProfilePicture.IsMain = true;

            await _userManager.UpdateAsync(user);
        }

        public Task DeactivateAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users.Include(u => u.ProfilePictureFiles).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new DeleteNotSucceedException($"Failed to delete user with ID {userId}.");
            }

            foreach (var imageFile in user.ProfilePictureFiles)
            {
                await _storage.DeleteAsync(imageFile.FileName, imageFile.PathName);
            }

            scope.Complete();
        }


        public async Task<GetAllUsersDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var userQuery = _userManager.Users
                .Include(u => u.ProfilePictureFiles)
                .AsNoTrackingWithIdentityResolution()
                .AsQueryable();

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, userQuery, cancellationToken);

            var users = await userQuery.ToListAsync(cancellationToken);

            var userDtos = new List<GetUserDTO>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var roles = await _roleManager.Roles
                    .Where(role => userRoles.Contains(role.Name ?? string.Empty))
                    .ToListAsync(cancellationToken);

                var userDto = new GetUserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName!,
                    LastName = user.LastName!,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    IsActive = user.IsActive,
                    DeactivatedAt = user.DeactivatedAt,
                    CreatedAt = user.CreatedAt,
                    Roles = roles.Select(r => new GetRoleDTO
                    {
                        Id = r.Id,
                        Name = r.Name!,
                        Description = r.Description!,
                        CreatedAt = r.CreatedAt
                    }).ToList(),
                    ProfilePictures = user.ProfilePictureFiles.Select(pp => new GetImageFileDTO
                    {
                        Id = pp.Id.ToString(),
                        FileName = pp.FileName,
                        PathName = pp.PathName,
                        IsMain = pp.IsMain,
                        CreatedAt = pp.CreatedAt
                    }).ToList()
                };

                userDtos.Add(userDto);
            }

            return new GetAllUsersDTO
            {
                TotalPages = paginationResult.TotalPages,
                TotalItems = paginationResult.TotalItems,
                PageSize = pageSize,
                Page = page,
                Users = userDtos,
            };
        }


        public async Task<GetUserDTO> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.ProfilePictureFiles).AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = await _roleManager.Roles
                .Where(role => userRoles.Contains(role.Name ?? string.Empty))
                .ToListAsync(cancellationToken);

            return new GetUserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                UserName = user.UserName!,
                Email = user.Email!,
                IsActive = user.IsActive,
                DeactivatedAt = user.DeactivatedAt,
                CreatedAt = user.CreatedAt,
                Roles = roles.Select(r => new GetRoleDTO
                {
                    Id = r.Id,
                    Name = r.Name!,
                    Description = r.Description!,
                    CreatedAt = r.CreatedAt
                }).ToList(),
                ProfilePictures = user.ProfilePictureFiles.Select(pp => new GetImageFileDTO
                {
                    Id = pp.Id.ToString(),
                    FileName = pp.FileName,
                    PathName = pp.PathName,
                    IsMain = pp.IsMain,
                    CreatedAt = pp.CreatedAt
                }).ToList(),
            };
        }

        public async Task<List<GetImageFileDTO>> GetImagesAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.ProfilePictureFiles).AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            if (user.ProfilePictureFiles.Count == 0) return [];

            return user.ProfilePictureFiles.Select(pp => new GetImageFileDTO
            {
                Id = pp.Id.ToString(),
                FileName = pp.FileName,
                PathName = pp.PathName,
                IsMain = pp.IsMain,
                CreatedAt = pp.CreatedAt
            }).ToList();
        }

        public async Task<List<GetRoleDTO>> GetRolesAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = await _roleManager.Roles
                .Where(role => userRoles.Contains(role.Name ?? string.Empty)).AsNoTracking()
                .ToListAsync(cancellationToken);

            var rolesWithUsers = new List<GetRoleDTO>();

            foreach (var role in roles)
            {

                rolesWithUsers.Add(new GetRoleDTO
                {
                    Id = role.Id,
                    Name = role.Name!,
                    Description = role.Description!,
                    CreatedAt = role.CreatedAt,
                });
            }

            return rolesWithUsers;
        }


        public async Task RemovePictureAsync(string userId, string pictureId, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await _userManager.Users
                .Include(u => u.ProfilePictureFiles)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user));
            }

            var picture = user.ProfilePictureFiles.FirstOrDefault(pp => pp.Id.ToString() == pictureId);

            if (picture == null)
            {
                throw new EntityNotFoundException(nameof(picture));
            }

            if (!user.ProfilePictureFiles.Remove(picture))
            {
                throw new DeleteNotSucceedException("Could not remove the picture from user's profile.");
            }

            if (_userProfilePictureFileWriteRepository.Delete(picture))
            {
                throw new DeleteNotSucceedException("Could not delete the picture from the repository.");
            }

            await _storage.DeleteAsync(picture.PathName, picture.FileName);

            await _userManager.UpdateAsync(user);

            scope.Complete();
        }


        public Task UpdateAsync(UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
