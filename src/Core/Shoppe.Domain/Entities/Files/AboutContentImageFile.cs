using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public class AboutContentImageFile : ContentImageFile
    {
        [ForeignKey(nameof(About))]
        public Guid AboutId { get; set; }
        public About About { get; set; } = null!;
    }
}
