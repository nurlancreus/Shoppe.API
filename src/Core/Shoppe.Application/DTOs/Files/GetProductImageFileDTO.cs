using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Files
{
    public class GetProductImageFileDTO
    {
        public string Id { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string PathName { get; set; } = null!;
        public bool IsMain { get; set; }
    }
}
