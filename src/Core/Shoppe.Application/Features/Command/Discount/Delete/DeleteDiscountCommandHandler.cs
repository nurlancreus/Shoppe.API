using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.DeleteDiscount
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommandRequest, DeleteDiscountCommandResponse>
    {
        private readonly IDiscountService _discountService;

        public DeleteDiscountCommandHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<DeleteDiscountCommandResponse> Handle(DeleteDiscountCommandRequest request, CancellationToken cancellationToken)
        {
            await _discountService.DeleteAsync((Guid)request.Id!, cancellationToken);

            return new DeleteDiscountCommandResponse
            {
                IsSuccess = true,
                Message = ResponseConst.DeletedSuccessMessage("Discount")
            };
        }
    }
}
