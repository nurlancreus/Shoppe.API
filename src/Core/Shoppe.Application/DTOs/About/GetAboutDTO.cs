using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.SocialMediaLink;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.About
{
    public record GetAboutDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Content { get; set; }
        public ICollection<GetContentFileDTO> ContentImages { get; set; } = [];
        public List<GetSocialMediaLinkDTO> SocialMediaLinks { get; set; } = [];
    }
}
