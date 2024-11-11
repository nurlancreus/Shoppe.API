using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class About : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        // public ICollection<AboutSection> Sections { get; set; } = [];
        public string? Content { get; set; }
        public ICollection<AboutContentImageFile> ContentImages { get; set; } = [];

        public ICollection<SocialMediaLink> SocialMediaLinks { get; set; } = [];
    }
}
