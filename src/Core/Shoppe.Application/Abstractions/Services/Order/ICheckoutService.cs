using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        Task CheckoutAsync(Guid basketId, CancellationToken cancellationToken = default);

    }
}
