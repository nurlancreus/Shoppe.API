using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment.PayPal
{
    public interface IPayPalWebhookService
    {
        Task<bool> VerifyWebhookAsync(string rawBody, IHeaderDictionary headers, CancellationToken cancellationToken = default);
    }

}
