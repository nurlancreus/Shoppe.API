using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Extensions.Helpers
{
    public static class StringHelpers
    {
        public static string SplitAndJoinString(string text, char divider = ',', char joiner = ' ')
        {
            string[] parts = text.Split(divider);

            return string.Join(joiner, parts);
        }
    }
}
