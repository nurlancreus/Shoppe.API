using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Discount.UpdateDiscount;
using System;

public class UpdateDiscountCommandRequestValidator : AbstractValidator<UpdateDiscountCommandRequest>
{
    private readonly IDiscountReadRepository _discountReadRepository;

    public UpdateDiscountCommandRequestValidator(IDiscountReadRepository discountReadRepository)
    {
        _discountReadRepository = discountReadRepository;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Discount Id is required.")
            .MustAsync(async (id, cancellationToken) => await _discountReadRepository.IsExistAsync(d => d.Id == id, cancellationToken))
            .WithMessage("The discount does not exist.");

        RuleFor(x => x.Name)
            .MustAsync(async (request, name, cancellationToken) =>
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var exists = await _discountReadRepository.IsExistAsync(
                        d => d.Name == name && d.Id != request.Id, cancellationToken);
                    return !exists;
                }
                return true;
            })
            .WithMessage("A discount with the same name already exists.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(DiscountConst.MaxDescLength).WithMessage($"Description must not exceed {DiscountConst.MaxDescLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0, 100).WithMessage("Discount percentage must be between 0% and 100%.")
            .When(x => x.DiscountPercentage.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after the start date.")
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue);
    }
}
