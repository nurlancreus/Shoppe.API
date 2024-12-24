using MediatR;
using Shoppe.Application.Abstractions.Services.Address;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Address.Shipping.Create
{
    public class CreateShippingAddressCommandHandler : IRequestHandler<CreateShippingAddressCommandRequest, CreateShippingAddressCommandResponse>
    {
        private readonly IShippingAddressService _addressService;

        public CreateShippingAddressCommandHandler(IShippingAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<CreateShippingAddressCommandResponse> Handle(CreateShippingAddressCommandRequest request, CancellationToken cancellationToken)
        {
            await _addressService.CreateShippingAsync(request.ToCreateShippingAddressDTO(), cancellationToken);

            return new CreateShippingAddressCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
