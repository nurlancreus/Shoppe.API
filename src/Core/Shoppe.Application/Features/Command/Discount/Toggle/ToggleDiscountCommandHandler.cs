using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.Toggle
{
    public class ToggleDiscountCommandHandler : IRequestHandler<ToggleDiscountCommandRequest, ToggleDiscountCommandResponse>
    {
        private readonly IDiscountService _discountService;

        public ToggleDiscountCommandHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<ToggleDiscountCommandResponse> Handle(ToggleDiscountCommandRequest request, CancellationToken cancellationToken)
        {
            await _discountService.ToggleDiscountAsync((Guid)request.Id!, cancellationToken);

            return new ToggleDiscountCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
