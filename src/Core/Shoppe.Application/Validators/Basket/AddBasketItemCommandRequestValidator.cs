using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Features.Command.Basket.AddBasketItem;

public class AddBasketItemCommandRequestValidator : AbstractValidator<AddBasketItemCommandRequest>
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IStockService _stockService;

    public AddBasketItemCommandRequestValidator(IProductReadRepository productReadRepository, IStockService stockService)
    {
        _productReadRepository = productReadRepository;
        _stockService = stockService;

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.")
            .MustAsync(ProductExists).WithMessage("The specified product does not exist.");

        RuleFor(x => x.Quantity)
            .Must(q => q.HasValue && q.Value > 0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x)
            .MustAsync(HaveSufficientStock).WithMessage("Insufficient product stock for the requested quantity.")
            .When(request => request.Quantity.HasValue); 
    }

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
    {
        return await _productReadRepository.IsExistAsync(p => p.Id == productId, cancellationToken);
    }

    private async Task<bool> HaveSufficientStock(AddBasketItemCommandRequest request, CancellationToken cancellationToken)
    {

        return await _stockService.IsStockAvailableAsync(request.ProductId, request.Quantity!.Value, cancellationToken);
    }
}
