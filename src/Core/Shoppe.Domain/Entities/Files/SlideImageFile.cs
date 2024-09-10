using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public class SlideImageFile : ImageFile
    {
        public Guid SlideId { get; set; }
        public Slide Slide { get; set; } = null!;
    }
}
