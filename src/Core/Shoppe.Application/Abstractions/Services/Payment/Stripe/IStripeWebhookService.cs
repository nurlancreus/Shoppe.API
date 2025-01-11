using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Payment.Stripe
{
    public interface IStripeWebHookService
    {
        Task ReceivePayloadAsync(string payload, IHeaderDictionary headers, CancellationToken cancellationToken = default);
    }
}
