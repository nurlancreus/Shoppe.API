using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Slide
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string URL { get; set; } = null!;
        public SlideImageFile SlideImageFile { get; set; } = null!;
    }
}
