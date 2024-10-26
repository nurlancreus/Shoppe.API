using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Sliders
{
    public class Slider : BaseEntity
    {
        public ICollection<Slide> Slides { get; set; } = [];
        public string Type { get; set; } = string.Empty;
    }
}
