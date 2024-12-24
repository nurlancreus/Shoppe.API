using Shoppe.Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Address
{
    public interface IBillingAddressService
    {
        Task CreateBillingAsync(CreateBillingAddressDTO createBillingAddressDTO, CancellationToken cancellationToken = default);
        Task UpdateBillingAsync(UpdateBillingAddressDTO UpdateBillingAddressDTO, CancellationToken cancellationToken = default);
        Task<GetAddressDTO> GetBillingAddressAsync (CancellationToken cancellationToken = default);
    }
}
