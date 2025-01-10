using Force.Crc32;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shoppe.Application.Abstractions.Services.Payment.PayPal;
using Shoppe.Application.Options.Payment;
using System.Security.Cryptography;
using System.Text;

public class PayPalWebhookService : IPayPalWebhookService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PayPalWebhookService> _logger;
    private readonly string _webhookId;

    public PayPalWebhookService(IHttpClientFactory httpClientFactory, IOptions<PaymentOptions> options, ILogger<PayPalWebhookService> logger)
    {
        _httpClient= httpClientFactory.CreateClient();
        _logger = logger;
        _webhookId = options.Value.PayPal.WebhookId; 
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
