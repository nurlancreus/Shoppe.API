using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Helpers
{
    public static class EnumHelpers
    {
        public static bool TryParseEnum<T>(string value, out T result, bool caseInsentitive = true) where T : struct, Enum
        {
            // Use Enum.TryParse with ignoreCase = true for case-insensitive parsing
            return Enum.TryParse(value, ignoreCase: caseInsentitive, result: out result);
        }
    }
}
