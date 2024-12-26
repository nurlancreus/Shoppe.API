using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Session
{
    public interface IJwtSession
    {
        string GetUserId();
        string GetUserName();
        string GetUserEmail();
        List<string> GetRoles();
        bool IsAuthenticated(bool throwException = false);
        bool IsAdmin();
        bool IsSuperAdmin();
        ApplicationUser GetUser();
        bool ValidateAdminAccess(bool throwException = true);
        bool ValidateSuperAdminAccess(bool throwException = true);
        bool ValidateAuthAccess(string id, bool throwException = true);
        bool ValidateRoleAccess(IEnumerable<string> roles, bool throwException = true);
     }
}
