using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Options.Token
{
    public class RefreshOptions
    {
        public const string Refresh = "Refresh";
        public int RefreshTokenLifeTimeInMinutes { get; set; }
    }
}
