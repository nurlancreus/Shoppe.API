using Shoppe.Application.DTOs.Role;
using Shoppe.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task CreateAsync(CreateRoleDTO createRoleDTO, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateRoleDTO updateRoleDTO, CancellationToken cancellationToken);
        Task DeleteAsync(string roleId, CancellationToken cancellationToken);
        Task AssignUsersToRoleAsync(string roleId, List<string> userNames, CancellationToken cancellationToken);
        Task<GetAllRolesDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<GetRoleDTO> GetAsync(string roleId, CancellationToken cancellationToken);
        Task<GetAllUsersDTO> GetUsersAsync(string roleId, int page, int pageSize, CancellationToken cancellationToken);
    }
}
