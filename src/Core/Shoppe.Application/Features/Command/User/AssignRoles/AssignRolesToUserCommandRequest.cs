using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.AssignRoles
{
    public class AssignRolesToUserCommandRequest : IRequest<AssignRolesToUserCommandResponse>
    {
        public string? UserId { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
