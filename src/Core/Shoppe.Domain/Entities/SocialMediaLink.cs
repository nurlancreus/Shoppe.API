using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class SocialMediaLink : BaseEntity
    {
        public string URL { get; set; } = string.Empty;
        public SocialPlatform SocialPlatform { get; set; }
        public Guid AboutId { get; set; }
    }
}
