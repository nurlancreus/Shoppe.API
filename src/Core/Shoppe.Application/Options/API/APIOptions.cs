using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Options.API
{
    public class APIOptions
    {
        public const string CountryAPI = "CountryAPI";
        public const string AmadeusAPI = "AmadeusAPI";

        public string BaseUrl { get; set; } = string.Empty;
        public float? Version { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiSecret { get; set; }
    }
}
