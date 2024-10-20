using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.AssignRoles
{
    public class AssignRolesToUserCommandHandler : IRequestHandler<AssignRolesToUserCommandRequest, AssignRolesToUserCommandResponse>
    {
        public Task<AssignRolesToUserCommandResponse> Handle(AssignRolesToUserCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
