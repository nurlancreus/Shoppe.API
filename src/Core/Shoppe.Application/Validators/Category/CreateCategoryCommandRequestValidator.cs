using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Category.CreateCategory;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Category
{
    public class CreateCategoryCommandRequestValidator : AbstractValidator<CreateCategoryCommandRequest>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;   
        public CreateCategoryCommandRequestValidator(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;

            RuleFor(category => category.Name)
              .NotEmpty()
              .WithMessage("Name is required.")
              .MaximumLength(CategoryConst.MaxNameLength)
              .WithMessage($"Name must be less than {CategoryConst.MaxNameLength} characters.")
              .MustAsync(async (name, cancellationToken) => !await _categoryReadRepository.IsExistAsync(c => c.Name == name, cancellationToken))
              .WithMessage("Category is already defined.");

            RuleFor(category => category.Description)
               .MaximumLength(CategoryConst.MaxDescLength)
               .WithMessage($"Description must be less than {CategoryConst.MaxDescLength} characters.")
               .When(category => !string.IsNullOrEmpty(category.Description));

            RuleFor(category => category.Type)
                .NotEmpty()
                .WithMessage("Category type is required.")
                .Must(type => Enum.TryParse(typeof(CategoryType), type, true, out _))
                .WithMessage("Invalid category type specified.");
        }

    }
}
