﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.ChangeProfilePicture
{
    public class ChangeProfilePictureCommandHandler : IRequestHandler<ChangeProfilePictureCommandRequest, ChangeProfilePictureCommandResponse>
    {
        public Task<ChangeProfilePictureCommandResponse> Handle(ChangeProfilePictureCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
