using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Domain.Entities.Sliders;

namespace Shoppe.Domain.Entities.Files
{
    public class SlideImageFile : ImageFile
    {
        public Slide Slide { get; set; } = null!;
    }
}
