using MediatR;
using Shoppe.Domain.Entities.Sections;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.SocialMediaLink;

namespace Shoppe.Application.Features.Command.About.Update
{
    public class UpdateAboutCommandRequest : IRequest<UpdateAboutCommandResponse>
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<CreateSectionDTO> Sections { get; set; } = [];
        public List<UpdateSectionDTO> UpdatedSections { get; set; } = [];
        public List<CreateSocialMediaLinkDTO> SocialMediaLinks { get; set; } = [];
    }
}
