using Shoppe.Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Address
{
    public interface IShippingAddressService
    {
        Task CreateShippingAsync(CreateShippingAddressDTO createShippingAddressDTO, CancellationToken cancellationToken = default);
        Task UpdateShippingAsync(UpdateShippingAddressDTO UpdateShippingAddressDTO, CancellationToken cancellationToken = default);
        Task<GetAddressDTO> GetShippingAddressAsync(CancellationToken cancellationToken = default);

    }
}
