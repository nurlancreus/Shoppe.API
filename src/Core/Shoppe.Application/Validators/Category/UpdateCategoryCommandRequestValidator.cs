using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Domain.Constants;
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
                .WithMessage("Category ID is required.")
                .MustAsync(async (id, cancellationToken) => await _categoryReadRepository.IsExistAsync(c => c.Id == id, cancellationToken))
                .WithMessage("The category does not exist.");

            RuleFor(category => category.Name)
                .MaximumLength(CategoryConst.MaxNameLength)
                .WithMessage($"Name must be less than {CategoryConst.MaxNameLength} characters.")
                .MustAsync(async (request, name, cancellationToken) =>
                    !await _categoryReadRepository.IsExistAsync(c => c.Name == name && c.Id != request.Id, cancellationToken))
                .WithMessage("Category with this name already exists.");

            RuleFor(category => category.Description)
                .MaximumLength(CategoryConst.MaxDescLength)
                .WithMessage($"Description must be less than {CategoryConst.MaxDescLength} characters.")
                .When(category => !string.IsNullOrEmpty(category.Description));
        }
    }
}
