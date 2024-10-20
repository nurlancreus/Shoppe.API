using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.Deactivate
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommandRequest, DeactivateUserCommandResponse>
    {
        public Task<DeactivateUserCommandResponse> Handle(DeactivateUserCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
