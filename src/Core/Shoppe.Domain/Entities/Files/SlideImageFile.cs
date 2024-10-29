using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Domain.Entities.Sliders;

namespace Shoppe.Domain.Entities.Files
{
    public class SlideImageFile : ApplicationFile
    {
        [ForeignKey(nameof(Slide))]
        public Guid SlideId { get; set; }
        public Slide Slide { get; set; } = null!;
    }
}
