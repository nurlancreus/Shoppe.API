using MediatR;
using Shoppe.Application.Abstractions.Services.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Address.Shipping.Get
{
    public class GetShippingAddressQueryHandler : IRequestHandler<GetShippingAddressQueryRequest, GetShippingAddressQueryResponse>
    {
        private readonly IShippingAddressService _addressService;

        public GetShippingAddressQueryHandler(IShippingAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<GetShippingAddressQueryResponse> Handle(GetShippingAddressQueryRequest request, CancellationToken cancellationToken)
        {
            var address = await _addressService.GetShippingAddressAsync(cancellationToken);

            return new GetShippingAddressQueryResponse
            {
                IsSuccess = true,
                Data = address,
            };
        }
    }
}
