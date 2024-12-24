using MediatR;
using Shoppe.Application.Abstractions.Services.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Address.Billing.Get
{
    public class GetBillingAddressQueryHandler : IRequestHandler<GetBillingAddressQueryRequest, GetBillingAddressQueryResponse>
    {
        private readonly IBillingAddressService _addressService;

        public GetBillingAddressQueryHandler(IBillingAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<GetBillingAddressQueryResponse> Handle(GetBillingAddressQueryRequest request, CancellationToken cancellationToken)
        {
            var address = await _addressService.GetBillingAddressAsync(cancellationToken);

            return new GetBillingAddressQueryResponse
            {
                IsSuccess = true,
                Data = address
            };
        }
    }
}
