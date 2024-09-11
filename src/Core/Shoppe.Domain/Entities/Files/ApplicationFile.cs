using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public class ApplicationFile : BaseEntity
    {
        public string FileName { get; set; } = null!;
        public string PathName { get; set; } = null!;

        [NotMapped]
        public override DateTime? UpdatedAt => null;

    }
}
