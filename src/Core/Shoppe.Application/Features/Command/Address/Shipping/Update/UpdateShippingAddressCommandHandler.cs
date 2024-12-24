using MediatR;
using Shoppe.Application.Abstractions.Services.Address;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Address.Shipping.Update
{
    public class UpdateShippingAddressCommandHandler : IRequestHandler<UpdateShippingAddressCommandRequest, UpdateShippingAddressCommandResponse>
    {
        private readonly IShippingAddressService _addressService;

        public UpdateShippingAddressCommandHandler(IShippingAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<UpdateShippingAddressCommandResponse> Handle(UpdateShippingAddressCommandRequest request, CancellationToken cancellationToken)
        {
            await _addressService.UpdateShippingAsync(request.ToUpdateShippingAddressDTO(), cancellationToken);

            return new UpdateShippingAddressCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
