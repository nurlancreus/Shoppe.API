using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Role;
using Shoppe.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task AssignRolesAsync(string userId, List<string> roles, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateUserDTO updateUserDTO, CancellationToken cancellationToken);
        Task DeleteAsync (string userId, CancellationToken cancellationToken);
        Task RemovePictureAsync (string userId, string pictureId, CancellationToken cancellationToken);
        Task ChangeProfilePictureAsync (string userId, string newPictureId, CancellationToken cancellationToken);
        Task DeactivateAsync (string userId, CancellationToken cancellationToken);
        Task<GetUserDTO> GetAsync(string userId, CancellationToken cancellationToken);
        Task<GetAllUsersDTO> GetAllAsync (int page, int pageSize, CancellationToken cancellationToken);
        Task<List<GetImageFileDTO>> GetImagesAsync (string userId, CancellationToken cancellationToken);
        Task<List<GetRoleDTO>> GetRolesAsync (string userId, CancellationToken cancellationToken);

    }
}
