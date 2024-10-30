using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.SocialMediaLink
{
    public record GetSocialMediaLinkDTO
    {
        public string Id { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string SocialPlatform { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
