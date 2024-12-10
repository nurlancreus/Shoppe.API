using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.AssignEntities
{
    public class AssignDiscountToEntitiesCommandHandler : IRequestHandler<AssignDiscountToEntitiesCommandRequest, AssignDiscountToEntitiesCommandResponse>
    {
        private readonly IDiscountService _discountService;

        public AssignDiscountToEntitiesCommandHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<AssignDiscountToEntitiesCommandResponse> Handle(AssignDiscountToEntitiesCommandRequest request, CancellationToken cancellationToken)
        {
            await _discountService.AssignDiscountToEntitiesAsync((Guid)request.Id!, request.ProductIds, request.CategoryIds, cancellationToken);

            return new AssignDiscountToEntitiesCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
