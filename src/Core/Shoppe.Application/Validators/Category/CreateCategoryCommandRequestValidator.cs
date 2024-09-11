using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Category.CreateCategory;
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
              .MustAsync(async (name, cancellationToken) => !await _categoryReadRepository.IsExist(c => c.Name == name, cancellationToken))
              .WithMessage("Category is already defined.");
        }

    }
}
