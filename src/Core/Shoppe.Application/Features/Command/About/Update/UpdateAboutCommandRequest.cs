using MediatR;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Application.DTOs.SocialMediaLink;
using Shoppe.Domain.Entities.Files;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Content;

namespace Shoppe.Application.Features.Command.About.Update
{
    public class UpdateAboutCommandRequest : IRequest<UpdateAboutCommandResponse>
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Content { get; set; }
        public List<CreateContentImageFileDTO> ContentImages { get; set; } = [];
        public List<CreateSocialMediaLinkDTO> SocialMediaLinks { get; set; } = [];
    }
}
