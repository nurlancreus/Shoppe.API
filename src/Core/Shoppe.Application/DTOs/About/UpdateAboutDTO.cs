using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.SocialMediaLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.About
{
    public record UpdateAboutDTO
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<CreateSectionDTO> Sections { get; set; } = [];
        public List<CreateSocialMediaLinkDTO> SocialMediaLinks { get; set; } = [];
    }
}
