using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Category.UpdateCategory;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Category
{
    public class UpdateCategoryCommandRequestValidator : AbstractValidator<UpdateCategoryCommandRequest>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public UpdateCategoryCommandRequestValidator(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;

            RuleFor(category => category.Id)
                .NotEmpty()
                .WithMessage("Category ID is required.");

            RuleFor(category => category.Name)
                .MaximumLength(CategoryConst.MaxNameLength)
                .WithMessage($"Name must be less than {CategoryConst.MaxNameLength} characters.")
                .MustAsync(async (name, cancellationToken) =>
                    !await _categoryReadRepository.IsExist(c => c.Name == name, cancellationToken))
                .WithMessage("Category with this name already exists.");

            RuleFor(category => category.Description)
                .MaximumLength(300)
                .WithMessage($"Description must be less than {300} characters.")
                .When(category => !string.IsNullOrEmpty(category.Description));

            RuleFor(category => category.Type)
                .NotEmpty()
                .WithMessage("Category type is required.")
                .Must(type => type == "Product" || type == "Blog")
                .WithMessage("Category type must be either 'Product' or 'Blog'.");
        }
    }
}
