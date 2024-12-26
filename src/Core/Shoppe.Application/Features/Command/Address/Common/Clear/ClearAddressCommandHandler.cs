using MediatR;
using Shoppe.Application.Abstractions.Services.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Address.Common.Clear
{
    public class ClearAddressCommandHandler : IRequestHandler<ClearAddressCommandRequest, ClearAddressCommandResponse>
    {
        private readonly IAddressService _addressService;

        public ClearAddressCommandHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<ClearAddressCommandResponse> Handle(ClearAddressCommandRequest request, CancellationToken cancellationToken)
        {
            await _addressService.ClearAsync(cancellationToken);

            return new ClearAddressCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
