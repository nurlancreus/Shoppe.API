using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Mail.Templates
{
    public interface IContactEmailTemplateService
    {
        string GenerateContactResponseTemplate(string recipientName, ContactSubject subject, string message);
        string GenerateContactReceivedTemplate(string recipientName, ContactSubject subject);
    }
}
