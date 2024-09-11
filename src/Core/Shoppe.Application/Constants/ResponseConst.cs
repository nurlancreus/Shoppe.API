using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Constants
{
    public static class ResponseConst
    {
        public static string AddedSuccessMessage(string entityName)
        {
            return $"{entityName} added successfully!";
        }

        public static string UpdatedSuccessMessage(string entityName)
        {
            return $"{entityName} updated successfully!";
        }

        public static string DeletedSuccessMessage(string entityName)
        {
            return $"{entityName} deleted successfully!";
        }
    }
}
