using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Role;
using Shoppe.Application.DTOs.User;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtSession _jwtSession;
        private readonly IPaginationService _paginationService;

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IJwtSession jwtSession, IPaginationService paginationService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtSession = jwtSession;
            _paginationService = paginationService;
        }

        public async Task AssignUsersToRoleAsync(string roleId, List<string> userNames, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) throw new EntityNotFoundException(nameof(role));

            var currentUsers = await _userManager.GetUsersInRoleAsync(role.Name ?? string.Empty);

            var newUsers = userNames.Except(currentUsers.Select(u => u.UserName)).ToList();
            var usersToRemove = currentUsers.Where(u => !userNames.Contains(u.UserName)).ToList();

            IdentityResult result;

            foreach (var userName in newUsers)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name!);
                    CheckUpdateSucceeded(result);
                }
            }

            foreach (var user in usersToRemove)
            {
                result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
                CheckUpdateSucceeded(result);
            }

            scope.Complete();
        }

        public async Task CreateAsync(CreateRoleDTO createRoleDTO, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            bool isRoleExist = await _roleManager.RoleExistsAsync(createRoleDTO.Name);
            if (isRoleExist)
            {
                throw new ValidationException($"Role with the name {createRoleDTO.Name} already exists.");
            }

            var role = new ApplicationRole
            {
                Name = createRoleDTO.Name,
                Description = createRoleDTO.Description,
            };

            var result = await _roleManager.CreateAsync(role);
            CheckCreateSucceeded(result);
            scope.Complete();

        }

        public async Task DeleteAsync(string roleId, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role));
            }

            var result = await _roleManager.DeleteAsync(role);
            CheckDeleteSucceeded(result);

            scope.Complete();

        }

        public async Task UpdateAsync(UpdateRoleDTO updateRoleDTO, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == updateRoleDTO.RoleId, cancellationToken);
            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role));
            }

            if (updateRoleDTO.Name is string name && role.Name != name)
            {
                var isRoleExist = await _roleManager.RoleExistsAsync(name);
                if (isRoleExist)
                {
                    throw new ValidationException($"Role with the name {name} already exists. Choose another name.");
                }
                role.Name = name;
            }

            if (updateRoleDTO.Description is string description && role.Description != description)
            {
                role.Description = description;
            }

            var result = await _roleManager.UpdateAsync(role);
            CheckUpdateSucceeded(result);

            scope.Complete();
        }

        public async Task<GetAllRolesDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            var roleQuery = _roleManager.Roles.AsNoTracking();
            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, roleQuery, cancellationToken);

            // Get a list of all role names in the paginated query
            var roleNames = paginationResult.PaginatedQuery.Select(role => role.Name).ToList();

            var usersByRole = new Dictionary<string, IList<ApplicationUser>>();

            foreach (var roleName in roleNames)
            {
                usersByRole[roleName!] = await _userManager.GetUsersInRoleAsync(roleName ?? string.Empty);
            }

            var roleDtos = paginationResult.PaginatedQuery.AsEnumerable().Select(role =>
            {
                var users = usersByRole[role.Name ?? string.Empty];

                return new GetRoleDTO
                {
                    Id = role.Id,
                    Name = role.Name!,
                    Description = role.Description!,
                    CreatedAt = role.CreatedAt,
                    Users = users.Select(u =>
                    {
                        //var userProfilePicture = u.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);
                        UserProfileImageFile? userProfilePicture = null;

                        return new GetUserDTO
                        {
                            Id = u.Id,
                            FirstName = u.FirstName!,
                            LastName = u.LastName!,
                            Email = u.Email!,
                            Phone = u.PhoneNumber!,
                            UserName = u.UserName!,
                            IsActive = u.IsActive,
                            CreatedAt = u.CreatedAt,
                            DeactivatedAt = u.DeactivatedAt,
                            ProfilePicture = userProfilePicture != null ? new GetImageFileDTO
                            {
                                Id = userProfilePicture.Id,
                                IsMain = userProfilePicture.IsMain,
                                FileName = userProfilePicture.FileName,
                                PathName = userProfilePicture.PathName,
                                CreatedAt = userProfilePicture.CreatedAt,
                            } : null
                        };
                    }).ToList()
                };
            }).ToList();

            return new GetAllRolesDTO
            {
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages,
                Roles = roleDtos
            };
        }

        public async Task<GetRoleDTO> GetAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role));
            }

            var users = await _userManager.GetUsersInRoleAsync(role.Name ?? string.Empty);

            return new GetRoleDTO
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description!,
                CreatedAt = role.CreatedAt,
                Users = users.Select(u =>
                {

                    //var userProfilePicture = u.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);
                    UserProfileImageFile? userProfilePicture = null;


                    return new GetUserDTO
                    {
                        Id = u.Id,
                        FirstName = u.FirstName!,
                        LastName = u.LastName!,
                        Email = u.Email!,
                        Phone = u.PhoneNumber!,
                        UserName = u.UserName!,
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt,
                        DeactivatedAt = u.DeactivatedAt,
                        ProfilePicture = userProfilePicture != null ? new GetImageFileDTO
                        {
                            Id = userProfilePicture.Id,
                            IsMain = userProfilePicture.IsMain,
                            FileName = userProfilePicture.FileName,
                            PathName = userProfilePicture.PathName,
                            CreatedAt = userProfilePicture.CreatedAt,
                        } : null
                    };
                }).ToList(),

            };
        }

        public async Task<GetAllUsersDTO> GetUsersAsync(string roleId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role));
            }

            var usersQuery = (await _userManager.GetUsersInRoleAsync(role.Name ?? string.Empty)).AsQueryable();

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, usersQuery, cancellationToken);

            var users = await paginationResult.PaginatedQuery.ToListAsync(cancellationToken);

            var userDtos = users.Select(u =>
            {

                // var userProfilePicture = u.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);
                UserProfileImageFile? userProfilePicture = null;

                return new GetUserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName!,
                    LastName = u.LastName!,
                    Email = u.Email!,
                    Phone = u.PhoneNumber!,
                    UserName = u.UserName!,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    DeactivatedAt = u.DeactivatedAt,
                    ProfilePicture = userProfilePicture != null ? new GetImageFileDTO
                    {
                        Id = userProfilePicture.Id,
                        IsMain = userProfilePicture.IsMain,
                        FileName = userProfilePicture.FileName,
                        PathName = userProfilePicture.PathName,
                        CreatedAt = userProfilePicture.CreatedAt,
                    } : null
                };
            }).ToList();

            return new GetAllUsersDTO
            {
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages,
                Users = userDtos,
            };
        }

        private static void CheckUpdateSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new UpdateNotSucceedException("Role update operation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        private static void CheckDeleteSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new DeleteNotSucceedException("Delete operation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        private static void CheckCreateSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new AddNotSucceedException("Role creation operation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

    }
}

/*
      public async Task<GetAllRolesDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
      {
          _jwtSession.ValidateAdminAccess();

          var roleQuery = _roleManager.Roles.AsNoTracking();
          var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, roleQuery, cancellationToken);

          // Get a list of all role names in the paginated query
          var rolesQuery = paginationResult.PaginatedQuery
                              .Include(r => r.UserRoles)
                                  .ThenInclude(ur => ur.User);

          var roleDtos = rolesQuery.AsEnumerable().Select(role =>
          {

              return new GetRoleDTO
              {
                  Id = role.Id,
                  Name = role.Name!,
                  Description = role.Description!,
                  CreatedAt = role.CreatedAt,
                  Users = role.UserRoles.Select(ur =>
                  {
                      var userProfilePicture = ur.User.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);

                      return new GetUserDTO
                      {
                          Id = ur.User.Id,
                          FirstName = ur.User.FirstName!,
                          LastName = ur.User.LastName!,
                          Email = ur.User.Email!,
                          Phone = ur.User.PhoneNumber!,
                          UserName = ur.User.UserName!,
                          IsActive = ur.User.IsActive,
                          CreatedAt = ur.User.CreatedAt,
                          DeactivatedAt = ur.User.DeactivatedAt,
                          ProfilePicture = userProfilePicture != null ? new GetImageFileDTO
                          {
                              Id = userProfilePicture.Id,
                              IsMain = userProfilePicture.IsMain,
                              FileName = userProfilePicture.FileName,
                              PathName = userProfilePicture.PathName,
                              CreatedAt = userProfilePicture.CreatedAt,
                          } : null
                      };
                  }).ToList()
              };
          }).ToList();

          return new GetAllRolesDTO
          {
              Page = paginationResult.Page,
              PageSize = paginationResult.PageSize,
              TotalItems = paginationResult.TotalItems,
              TotalPages = paginationResult.TotalPages,
              Roles = roleDtos
          };
      }


      public async Task<GetRoleDTO> GetAsync(string roleId, CancellationToken cancellationToken)
      {
          var role = await _roleManager.Roles
                              .Include(r => r.UserRoles)
                                  .ThenInclude(ur => ur.User)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

          if (role == null)
          {
              throw new EntityNotFoundException(nameof(role));
          }



          return new GetRoleDTO
          {
              Id = role.Id,
              Name = role.Name!,
              Description = role.Description!,
              CreatedAt = role.CreatedAt,
              Users = role.UserRoles.Select(ur =>
              {

                  var userProfilePicture = ur.User.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);

                  return new GetUserDTO
                  {
                      Id = ur.User.Id,
                      FirstName = ur.User.FirstName!,
                      LastName = ur.User.LastName!,
                      Email = ur.User.Email!,
                      Phone = ur.User.PhoneNumber!,
                      UserName = ur.User.UserName!,
                      IsActive = ur.User.IsActive,
                      CreatedAt = ur.User.CreatedAt,
                      DeactivatedAt = ur.User.DeactivatedAt,
                      ProfilePicture = userProfilePicture != null ? new GetImageFileDTO
                      {
                          Id = userProfilePicture.Id,
                          IsMain = userProfilePicture.IsMain,
                          FileName = userProfilePicture.FileName,
                          PathName = userProfilePicture.PathName,
                          CreatedAt = userProfilePicture.CreatedAt,
                      } : null
                  };
              }).ToList(),

          };
      }

public async Task<GetAllUsersDTO> GetUsersAsync(string roleId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var role = await _roleManager.Roles.Include(r => r.UserRoles).ThenInclude(ur => ur.User).FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role));
            }

            var usersQuery = role.UserRoles.Select(ur => ur.User).AsQueryable();

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, usersQuery, cancellationToken);

            var users = await paginationResult.PaginatedQuery.ToListAsync(cancellationToken);

            var userDtos = users.Select(u =>
            {
                var userProfilePicture = u.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);

                return new GetUserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName!,
                    LastName = u.LastName!,
                    Email = u.Email!,
                    Phone = u.PhoneNumber!,
                    UserName = u.UserName!,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    DeactivatedAt = u.DeactivatedAt,
                    ProfilePicture = userProfilePicture != null ? new GetImageFileDTO
                    {
                        Id = userProfilePicture.Id,
                        IsMain = userProfilePicture.IsMain,
                        FileName = userProfilePicture.FileName,
                        PathName = userProfilePicture.PathName,
                        CreatedAt = userProfilePicture.CreatedAt,
                    } : null
                };
            }).ToList();

            return new GetAllUsersDTO
            {
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages,
                Users = userDtos,
            };
        }
      */
