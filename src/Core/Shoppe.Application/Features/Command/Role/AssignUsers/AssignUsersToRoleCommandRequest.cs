using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Role.AssignUsers
{
    public class AssignUsersToRoleCommandRequest : IRequest<AssignUsersToRoleCommandResponse>
    {
        public string? RoleId { get; set; }
        public List<string> UserNames { get; set; } = [];
    }
}
