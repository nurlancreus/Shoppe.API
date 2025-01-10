

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
