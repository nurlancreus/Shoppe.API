using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Mail.Templates
{
    public interface IAccountEmailTemplateService
    {
        string GeneratePasswordResetTemplate(string recipientName, string resetLink);
        string GeneratePasswordChangedTemplate(string recipientName);
        string GenerateAccountCreatedTemplate(string recipientName);
    }
}
