using MediatR;
using Shoppe.Application.Abstractions.Services.Address;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Address.Billing.Create
{
    public class CreateBillingAddressCommandHandler : IRequestHandler<CreateBillingAddressCommandRequest, CreateBillingAddressCommandResponse>
    {
        private readonly IBillingAddressService _addressService;

        public CreateBillingAddressCommandHandler(IBillingAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<CreateBillingAddressCommandResponse> Handle(CreateBillingAddressCommandRequest request, CancellationToken cancellationToken)
        {
            await _addressService.CreateBillingAsync(request.ToCreateBillingAddressDTO(), cancellationToken);

            return new CreateBillingAddressCommandResponse
            {
                IsSuccess = true
            };
        }
    }
}
