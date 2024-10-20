using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Role.AssignUsers
{
    public class AssignUsersToRoleCommandHandler : IRequestHandler<AssignUsersToRoleCommandRequest, AssignUsersToRoleCommandResponse>
    {
        public Task<AssignUsersToRoleCommandResponse> Handle(AssignUsersToRoleCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
