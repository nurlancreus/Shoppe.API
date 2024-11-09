using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Extensions.Helpers
{
    public static class UrlHelpers
    {
        public static bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

    }
}
