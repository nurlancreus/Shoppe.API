using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.RemoveProfilePicture
{
    public class RemoveProfilePictureCommandHandler : IRequestHandler<RemoveProfilePictureCommandRequest, RemoveProfilePictureCommandResponse>
    {
        public Task<RemoveProfilePictureCommandResponse> Handle(RemoveProfilePictureCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
