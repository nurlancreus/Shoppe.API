using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Sections
{
    public class AboutSection : Section
    {
        public ICollection<AboutSectionImageFile> SectionImageFiles { get; set; } = [];

    }
}
