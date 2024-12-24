using MediatR;
using Shoppe.Application.Abstractions.Services.Address;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Address.Billing.Update
{
    public class UpdateBillingAddressCommandHandler : IRequestHandler<UpdateBillingAddressCommandRequest, UpdateBillingAddressCommandResponse>
    {
        private readonly IBillingAddressService _addressService;

        public UpdateBillingAddressCommandHandler(IBillingAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<UpdateBillingAddressCommandResponse> Handle(UpdateBillingAddressCommandRequest request, CancellationToken cancellationToken)
        {
            await _addressService.UpdateBillingAsync(request.ToUpdateBillingAddressDTO(), cancellationToken);

            return new UpdateBillingAddressCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
