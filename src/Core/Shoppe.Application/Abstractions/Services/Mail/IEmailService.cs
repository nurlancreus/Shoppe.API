using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Mail;

namespace Shoppe.Application.Abstractions.Services.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body);
        Task SendEmailWithAttachmentsAsync(RecipientDetailsDTO recipientDetails, string subject, string body, IFormFileCollection attachments);
        Task SendBulkEmailAsync(IEnumerable<RecipientDetailsDTO> recipientDetails, string subject, string body);
        Task SendTemplatedEmailAsync(RecipientDetailsDTO recipientDetails, string templateName, object templateData);
        Task ScheduleEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body, DateTime scheduleTime);
        Task SendEmailWithCustomHeadersAsync(RecipientDetailsDTO recipientDetails, string subject, string body, Dictionary<string, string> headers);
    }
}
