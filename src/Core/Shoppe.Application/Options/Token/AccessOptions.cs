using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Options.Token
{
    public class AccessOptions
    {
        public string Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string SecurityKey { get; set; } = null!;
        public int AccessTokenLifeTimeInMinutes { get; set; }
    }
}
