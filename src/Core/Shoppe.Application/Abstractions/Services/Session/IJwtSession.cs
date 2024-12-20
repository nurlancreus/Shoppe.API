﻿using Shoppe.Domain.Entities.Identity;
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
        bool IsAuthenticated();
        bool IsAdmin();
        bool IsSuperAdmin();
        ApplicationUser GetUser();
        void ValidateAdminAccess();
        void ValidateSuperAdminAccess();
        void ValidateAuthAccess(string id);
        void ValidateRoleAccess(IEnumerable<string> roles);
     }
}
