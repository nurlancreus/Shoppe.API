using Microsoft.Extensions.Options;
using Shoppe.Application.Options.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Payment.PayPal
{
    public sealed class PaypalClient(IOptions<PaymentOptions> options, IHttpClientFactory httpClientFactory)
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly PaymentOptions paymentOptions = options.Value;
        public string ClientId => paymentOptions.PayPal.ClientId;
        public string ClientSecret => paymentOptions.PayPal.ClientSecret;
        public string Mode => paymentOptions.PayPal.Mode;


        public string BaseUrl => Mode == "Live"
            ? "https://api-m.paypal.com"
            : "https://api-m.sandbox.paypal.com";

        private async Task<AuthResponse?> AuthenticateAsync(CancellationToken cancellationToken = default)
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));

            var content = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials")
        };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseUrl}/v1/oauth2/token"),
                Method = HttpMethod.Post,
                Headers =
            {
                { "Authorization", $"Basic {auth}" }
            },
                Content = new FormUrlEncodedContent(content)
            };

            var httpResponse = await _httpClient.SendAsync(request, cancellationToken);
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonSerializer.Deserialize<AuthResponse>(jsonResponse);

            return response;
        }

        public async Task<CreateOrderResponse?> CreateOrderAsync(string value, string currency, string reference, CancellationToken cancellationToken = default)
        {
            var auth = await AuthenticateAsync(cancellationToken);

            var request = new CreateOrderRequest
            {
                Intent = "CAPTURE",
                PurchaseUnits =
            [
                new()
                {
                    ReferenceId = reference,
                    Amount = new Amount
                    {
                        CurrencyCode = currency,
                        Value = value
                    }
                }
            ]
            };

            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth?.AccessToken}");

            var httpResponse = await _httpClient.PostAsJsonAsync($"{BaseUrl}/v2/checkout/orders", request, cancellationToken);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonSerializer.Deserialize<CreateOrderResponse>(jsonResponse);

            return response;
        }

        public async Task<CreateOrderResponse?> GetOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var auth = await AuthenticateAsync(cancellationToken);

            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth?.AccessToken}");

            var httpResponse = await _httpClient.GetAsync($"{BaseUrl}/v2/checkout/orders/{orderId}", cancellationToken);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonSerializer.Deserialize<CreateOrderResponse>(jsonResponse);

            return response;
        }

        public async Task<CaptureOrderResponse?> CaptureOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var auth = await AuthenticateAsync(cancellationToken);

            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth?.AccessToken}");

            var httpContent = new StringContent("", Encoding.Default, "application/json");

            var httpResponse = await _httpClient.PostAsync($"{BaseUrl}/v2/checkout/orders/{orderId}/capture", httpContent, cancellationToken);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonSerializer.Deserialize<CaptureOrderResponse>(jsonResponse);

            return response;
        }

        public async Task<bool> CancelOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var auth = await AuthenticateAsync(cancellationToken) ?? throw new InvalidOperationException("Failed to authenticate with PayPal.");

            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.AccessToken}");

            var httpResponse = await _httpClient.DeleteAsync($"{BaseUrl}/v2/checkout/orders/{orderId}", cancellationToken);

            return httpResponse.IsSuccessStatusCode;
        }

    }

    public sealed class AuthResponse
    {
        public string Scope { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string AppId { get; set; }
        public int ExpiresIn { get; set; }
        public string Nonce { get; set; }
    }

    public sealed class CreateOrderRequest
    {
        public string Intent { get; set; }
        public List<PurchaseUnit> PurchaseUnits { get; set; } = [];
    }

    public sealed class CreateOrderResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public List<Link> Links { get; set; } = [];
    }

    public sealed class CaptureOrderResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public PaymentSource PaymentSource { get; set; }
        public List<PurchaseUnit> PurchaseUnits { get; set; }
        public Payer Payer { get; set; }
        public List<Link> Links { get; set; }
    }

    public sealed class PurchaseUnit
    {
        public Amount Amount { get; set; }
        public string ReferenceId { get; set; }
        public Shipping Shipping { get; set; }
        public Payments Payments { get; set; }
    }

    public sealed class Payments
    {
        public List<Capture> Captures { get; set; }
    }

    public sealed class Shipping
    {
        public Address Address { get; set; }
    }

    public class Capture
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public Amount Amount { get; set; }
        public SellerProtection SellerProtection { get; set; }
        public bool FinalCapture { get; set; }
        public string DisbursementMode { get; set; }
        public SellerReceivableBreakdown SellerReceivableBreakdown { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Amount
    {
        public string CurrencyCode { get; set; }
        public string Value { get; set; }
    }

    public sealed class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }

    public sealed class Name
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
    }

    public sealed class SellerProtection
    {
        public string Status { get; set; }
        public List<string> DisputeCategories { get; set; }
    }

    public sealed class SellerReceivableBreakdown
    {
        public Amount GrossAmount { get; set; }
        public PaypalFee PaypalFee { get; set; }
        public Amount NetAmount { get; set; }
    }

    public sealed class Paypal
    {
        public Name Name { get; set; }
        public string EmailAddress { get; set; }
        public string AccountId { get; set; }
    }

    public sealed class PaypalFee
    {
        public string CurrencyCode { get; set; }
        public string Value { get; set; }
    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AdminArea2 { get; set; }
        public string AdminArea1 { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }

    public sealed class Payer
    {
        public Name Name { get; set; }
        public string EmailAddress { get; set; }
        public string PayerId { get; set; }
    }

    public sealed class PaymentSource
    {
        public Paypal Paypal { get; set; }
    }

}
