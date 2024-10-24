﻿using Shoppe.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public class AboutSectionImageFile : ImageFile
    {
        [ForeignKey(nameof(Section))]
        public Guid SectionId { get; set; }
        public AboutSection Section { get; set; } = null!;
    }
}
