using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.UpdateDiscount
{
    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommandRequest, UpdateDiscountCommandResponse>
    {
        private readonly IDiscountService _discountService;

        public UpdateDiscountCommandHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<UpdateDiscountCommandResponse> Handle(UpdateDiscountCommandRequest request, CancellationToken cancellationToken)
        {
            await _discountService.UpdateAsync(request.ToUpdateDiscountDTO(), cancellationToken);

            return new UpdateDiscountCommandResponse
            {
                IsSuccess = true,
                Message = ResponseConst.UpdatedSuccessMessage("Discount")
            };
        }
    }
}
