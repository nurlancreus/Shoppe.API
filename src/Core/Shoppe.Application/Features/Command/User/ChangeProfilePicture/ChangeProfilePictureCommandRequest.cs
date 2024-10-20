using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.ChangeProfilePicture
{
    public class ChangeProfilePictureCommandRequest : IRequest<ChangeProfilePictureCommandResponse>
    {
        public string? UserId { get; set; }
        public string? NewProfilePictureId { get; set;}
    }
}
