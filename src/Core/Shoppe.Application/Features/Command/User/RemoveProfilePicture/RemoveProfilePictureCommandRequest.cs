using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.RemoveProfilePicture
{
    public class RemoveProfilePictureCommandRequest : IRequest<RemoveProfilePictureCommandResponse>
    {
        public string? UserId { get; set; }
        public string? PictureId { get; set;}
    }
}
