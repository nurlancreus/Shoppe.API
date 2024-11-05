using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.ChangeCover
{
    public class ChangeCoverCommandRequest : IRequest<ChangeCoverCommandResponse>
    {
        public Guid? BlogId { get; set; }
        public Guid? NewImageId { get; set; }
        public IFormFile? NewImageFile { get; set; }

    }
}
