using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Features.Command.Basket.AddBasketItem;

public class AddBasketItemCommandRequestValidator : AbstractValidator<AddBasketItemCommandRequest>
{
    private readonly IProductReadRepository _productReadRepository;

    public AddBasketItemCommandRequestValidator(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;

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
        var product = await _productReadRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product == null)
        {
            return false;
        }

        return product.Stock >= request.Quantity!.Value; 
    }
}
