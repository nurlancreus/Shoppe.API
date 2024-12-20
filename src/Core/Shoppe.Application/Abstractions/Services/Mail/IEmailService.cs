﻿using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Mail;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body);
        Task SendEmailWithAttachmentsAsync(RecipientDetailsDTO recipientDetails, string subject, string body, IFormFileCollection attachments);
        Task SendBulkEmailAsync(IEnumerable<RecipientDetailsDTO> recipientDetails, string subject, string body);
        Task SendTemplatedEmailAsync(RecipientDetailsDTO recipientDetails, string templateName, object templateData);
        Task<MailStatus> GetEmailStatusAsync(string emailId);
        Task ScheduleEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body, DateTime scheduleTime);
        Task SendEmailWithCustomHeadersAsync(RecipientDetailsDTO recipientDetails, string subject, string body, Dictionary<string, string> headers);
    }
}
