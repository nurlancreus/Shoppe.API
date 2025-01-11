using Force.Crc32;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Abstractions.Services.Payment.PayPal;
using Shoppe.Application.DTOs.Payment;
using Shoppe.Application.Options.Payment;
using Shoppe.Domain.Constants.EventTypes;
using Shoppe.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace Shoppe.Infrastructure.Concretes.Services.Payment.PayPal
{
    public class PayPalWebHookService : IPayPalWebHookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _webhookId;
        private readonly ILogger<PayPalWebHookService> _logger;
        private readonly IPaymentEventService _paymentEventService;
        public PayPalWebHookService(IHttpClientFactory httpClientFactory, IOptions<PaymentOptions> options, ILogger<PayPalWebHookService> logger, IPaymentEventService paymentEventService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _webhookId = options.Value.PayPal.WebhookId;
            _paymentEventService = paymentEventService;
        }

        public async Task ReceivePayloadAsync(string payload, IHeaderDictionary headers, CancellationToken cancellationToken = default)
        {
            if (!await VerifyWebhookAsync(payload, headers, cancellationToken))
            {
                _logger.LogWarning("Invalid PayPal webhook signature.");
                throw new PaymentWebHookException("Invalid PayPal webhook signature.");
            }

            var notification = System.Text.Json.JsonSerializer.Deserialize<PayPalWebHookNotification>(payload);

            if (notification == null)
            {
                _logger.LogWarning("Failed to deserialize PayPal webhook payload.");
                throw new PaymentWebHookException("Invalid payload format.");
            }

            switch (notification.EventType)
            {
                case PayPalWebhookEventTypes.PAYMENT_CAPTURE_COMPLETED:
                    var completedPaymentOrderId = notification.Resource.ParentPayment;
                    await _paymentEventService.PaymentSucceededAsync(completedPaymentOrderId, cancellationToken);
                    break;
                case PayPalWebhookEventTypes.PAYMENT_CAPTURE_DECLINED:
                    var declinedPaymentOrderId = notification.Resource.ParentPayment;
                    await _paymentEventService.PaymentFailedAsync(declinedPaymentOrderId, cancellationToken);
                    break;
                case PayPalWebhookEventTypes.PAYMENT_ORDER_CANCELLED:
                    var cancelledPaymentOrderId = notification.Resource.ParentPayment;
                    await _paymentEventService.PaymentCanceledAsync(cancelledPaymentOrderId, cancellationToken);
                    break;
                case PayPalWebhookEventTypes.PAYMENT_REFUND_COMPLETED:
                    var refundedPaymentOrderId = notification.Resource.ParentPayment;
                    await _paymentEventService.PaymentRefundedAsync(refundedPaymentOrderId, cancellationToken);
                    break;
                default:
                    _logger.LogWarning($"Unsupported PayPal webhook event type: {notification.EventType}");
                    throw new PaymentWebHookException($"Unsupported PayPal webhook event type: {notification.EventType}", notification.EventType);
            }
        }

        public async Task<bool> VerifyWebhookAsync(string rawBody, IHeaderDictionary headers, CancellationToken cancellationToken)
        {
            var transmissionId = headers["paypal-transmission-id"].ToString();
            var timeStamp = headers["paypal-transmission-time"].ToString();
            var certUrl = headers["paypal-cert-url"].ToString();
            var receivedSignature = headers["paypal-transmission-sig"].ToString();

            if (string.IsNullOrEmpty(transmissionId) || string.IsNullOrEmpty(timeStamp) || string.IsNullOrEmpty(certUrl) || string.IsNullOrEmpty(receivedSignature))
            {
                _logger.LogWarning("Missing PayPal headers.");
                return false;
            }

            var crc32 = ComputeCRC32(rawBody);
            var message = $"{transmissionId}|{timeStamp}|{_webhookId}|{crc32}";

            var certPem = await DownloadCertificateAsync(certUrl, cancellationToken);
            var publicKey = LoadPublicKey(certPem);

            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(publicKey, out _);

            var signatureBytes = Convert.FromBase64String(receivedSignature);
            var dataBytes = Encoding.UTF8.GetBytes(message);

            return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        private async Task<string> DownloadCertificateAsync(string url, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        private static byte[] LoadPublicKey(string certPem)
        {
            var certBytes = Encoding.UTF8.GetBytes(certPem);
            using var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certBytes);
            return cert.GetPublicKey();
        }

        private static string ComputeCRC32(string data)
        {
            using var crc32 = new Crc32Algorithm();
            var bytes = Encoding.UTF8.GetBytes(data);
            var hash = crc32.ComputeHash(bytes);
            return BitConverter.ToUInt32(hash, 0).ToString();
        }
    }
}
