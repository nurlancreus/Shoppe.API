using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Discount.CreateDiscount;

public class CreateDiscountCommandRequestValidator : AbstractValidator<CreateDiscountCommandRequest>
{
    private readonly IDiscountReadRepository _discountReadRepository;

    public CreateDiscountCommandRequestValidator(IDiscountReadRepository discountReadRepository)
    {
        _discountReadRepository = discountReadRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Discount name is required.")
            .MaximumLength(DiscountConst.MaxNameLength).WithMessage($"Discount name must not exceed {DiscountConst.MaxNameLength} characters.")
            .MustAsync(async (name, cancellationToken) => !await _discountReadRepository.IsExistAsync(d => d.Name == name, cancellationToken))
            .WithMessage("A discount with the same name already exists.");

        RuleFor(x => x.Description)
            .MaximumLength(DiscountConst.MaxDescLength).WithMessage($"Description must not exceed {DiscountConst.MaxDescLength} characters.");

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0, 100).WithMessage("Discount percentage must be between 0% and 100%.");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after the start date.");
    }
}
