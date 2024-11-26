using MediatR;
using Shoppe.Application.Abstractions.Services;
using Enums = Shoppe.Domain.Enums;

namespace Shoppe.Application.Features.Command.Discount.AssignDiscount
{
    public class AssignDiscountCommandHandler : IRequestHandler<AssignDiscountCommandRequest, AssignDiscountCommandResponse>
    {
        private readonly IDiscountService _discountService;

        public AssignDiscountCommandHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<AssignDiscountCommandResponse> Handle(AssignDiscountCommandRequest request, CancellationToken cancellationToken)
        {

            if (Enum.TryParse(request.EntityType, true, out Enums.EntityType result))
            {

                await _discountService.AssignDiscountAsync((Guid)request.EntityId!, (Guid)request.DiscountId!, result, cancellationToken);
            }
            else throw new InvalidOperationException("Invalid entity type.");

            return new AssignDiscountCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
