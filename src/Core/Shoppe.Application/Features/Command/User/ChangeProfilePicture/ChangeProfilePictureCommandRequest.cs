using MediatR;
using Microsoft.AspNetCore.Http;
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
        public string? NewImageId { get; set; }
        public IFormFile? NewImageFile { get; set; }
    }
}
