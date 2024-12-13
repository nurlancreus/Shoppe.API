using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.BasketItemRepos;
using Shoppe.Application.Abstractions.Repositories.BasketRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Features.Command.Basket.UpdateItemQuantity;

public class UpdateItemQuantityCommandRequestValidator : AbstractValidator<UpdateItemQuantityCommandRequest>
{
    private readonly IBasketItemReadRepository _basketItemReadRepository;
    private readonly IProductReadRepository _productReadRepository;

    public UpdateItemQuantityCommandRequestValidator(IBasketItemReadRepository basketItemReadRepository, IProductReadRepository productReadRepository)
    {
        _basketItemReadRepository = basketItemReadRepository;
        _productReadRepository = productReadRepository;

        RuleFor(x => x.BasketItemId)
            .NotEmpty().WithMessage("Basket item ID is required.")
            .MustAsync(ExistInBasket).WithMessage("The basket item does not exist.");

        RuleFor(x => x)
            .MustAsync(HaveSufficientStock).WithMessage("Insufficient product stock for the requested quantity.");
    }

    // Method to check if the basket item exists
    private async Task<bool> ExistInBasket(Guid? basketItemId, CancellationToken cancellationToken)
    {
        return await _basketItemReadRepository.IsExistAsync(b => b.Id == basketItemId, cancellationToken);
    }

    private async Task<bool> HaveSufficientStock(UpdateItemQuantityCommandRequest request, CancellationToken cancellationToken)
    {
        var basketItem = await _basketItemReadRepository.GetByIdAsync((Guid)request.BasketItemId!, cancellationToken);

        if (basketItem == null)
        {
            return false;
        }

        var product = await _productReadRepository.GetByIdAsync(basketItem.ProductId, cancellationToken);

        if (product == null)
        {
            return false;
        }

        var newQuantity = request.Quantity ?? basketItem.Quantity + (request.Increment == true ? 1 : -1);

        return product.Stock >= newQuantity;
    }
}
