﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Shoppe.Application.Abstractions.Services.Mail;
using Shoppe.Application.DTOs.Mail;
using Shoppe.Application.Options.Mail;
using System.Text.Json;

namespace Shoppe.Infrastructure.Concretes.Services.Mail
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailConfig;

        public EmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailConfig = emailOptions.Value;
        }

        public async Task ScheduleEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body, DateTime scheduleTime)
        {
            var delay = scheduleTime - DateTime.Now;
            if (delay <= TimeSpan.Zero)
                throw new ArgumentException("Scheduled time must be in the future.");

            await Task.Delay(delay); // Simulate delay for scheduling
            await SendEmailAsync(recipientDetails, subject, body);
        }

        public async Task SendBulkEmailAsync(IEnumerable<RecipientDetailsDTO> recipientsDetails, string subject, string body)
        {
            var message = new MessageDTO
            {
                Recipients = recipientsDetails.Select(r => new MailboxAddress(r.Name, r.Email)).ToList(),
                Subject = subject,
                Content = body
            };

            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        public async Task SendEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body)
        {
            var message = new MessageDTO
            {
                To = new MailboxAddress(recipientDetails.Name, recipientDetails.Email),
                Subject = subject,
                Content = body
            };

            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        public async Task SendEmailWithAttachmentsAsync(RecipientDetailsDTO recipientDetails, string subject, string body, IFormFileCollection attachments)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));
            message.To.Add(new MailboxAddress(recipientDetails.Name, recipientDetails.Email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };

            foreach (var attachment in attachments)
            {
                using var stream = new MemoryStream();
                await attachment.CopyToAsync(stream);
                bodyBuilder.Attachments.Add(attachment.FileName, stream.ToArray(), ContentType.Parse(attachment.ContentType));
            }

            message.Body = bodyBuilder.ToMessageBody();
            await SendAsync(message);
        }


        public async Task SendEmailWithCustomHeadersAsync(RecipientDetailsDTO recipientDetails, string subject, string body, Dictionary<string, string> headers)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));
            message.To.Add(new MailboxAddress(recipientDetails.Name, recipientDetails.Email));
            message.Subject = subject;

            foreach (var header in headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            await SendAsync(message);
        }

        public async Task SendTemplatedEmailAsync(RecipientDetailsDTO recipientDetails, string templateName, object templateData)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{templateName}.html");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Email template not found.", templatePath);

            var templateContent = await File.ReadAllTextAsync(templatePath);
            var body = ReplaceTemplatePlaceholders(templateContent, templateData);

            await SendEmailAsync(recipientDetails, $"Subject from {templateName}", body);
        }

        private MimeMessage CreateEmailMessage(MessageDTO message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));

            if (message.To != null)
            {
                emailMessage.To.Add(message.To);
            }
            else if (message.Recipients?.Count > 0)
            {
                emailMessage.To.AddRange(message.Recipients);
            }

            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log an error or rethrow the exception
                Console.WriteLine($"Email sending failed: {ex.Message}");
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        private static string ReplaceTemplatePlaceholders(string template, object templateData)
        {
            var jsonData = JsonSerializer.Serialize(templateData);
            var dataDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData);

            foreach (var placeholder in dataDictionary)
            {
                template = template.Replace($"{{{{ {placeholder.Key} }}}}", placeholder.Value);
            }

            return template;
        }
    }
}
