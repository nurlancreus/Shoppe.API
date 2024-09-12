using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Options.Token
{
    public class TokenOptions
    {
        public AccessOptions Access { get; set; } = null!;
        public RefreshOptions Refresh { get; set; } = null!;
    }
}
