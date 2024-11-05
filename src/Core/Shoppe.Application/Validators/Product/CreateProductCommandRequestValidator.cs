using FluentValidation;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Product
{
    public class CreateProductCommandRequestValidator : AbstractValidator<CreateProductCommandRequest>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IDiscountReadRepository _discountReadRepository;
        public CreateProductCommandRequestValidator(ICategoryReadRepository categoryReadRepository, IDiscountReadRepository discountReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
            _discountReadRepository = discountReadRepository;

            // Validate Name
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(ProductConst.MaxNameLength)
                .WithMessage($"Name must be less than {ProductConst.MaxNameLength} characters.");

            RuleFor(product => product.Info)
                .NotEmpty()
                .WithMessage("Info is required.")
                .MaximumLength(ProductConst.MaxInfoLength)
                .WithMessage($"Info must be less than {ProductConst.MaxInfoLength} characters.");

            // Validate Description
            RuleFor(product => product.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(ProductConst.MaxDescLength)
                .WithMessage($"Description must be less than {ProductConst.MaxDescLength} characters.");

            // Validate Price
            RuleFor(product => product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            // Validate Stock
            RuleFor(product => product.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            // Validate Weigth, Height, and Width
            RuleFor(product => product.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than zero.");

            RuleFor(product => product.Height)
                .GreaterThan(0).WithMessage("Height must be greater than zero.");

            RuleFor(product => product.Width)
                .GreaterThan(0).WithMessage("Width must be greater than zero.");
            ;
            RuleForEach(product => product.Materials)
                    .Must(material => EnumHelpers.IsDefinedEnum<Material>(material, out _))
                    .WithMessage("Material must be a valid enum value.");

            RuleForEach(product => product.Colors)
                .Must(color => EnumHelpers.IsDefinedEnum<Color>(color, out _))
                .WithMessage("Color must be a valid enum value.");

            RuleForEach(product => product.Categories)
                .MustAsync(async (name, cancellationToken) => await _categoryReadRepository.IsExistAsync(c => c.Name == name, cancellationToken))
                .WithMessage("Category must be defined");

            When(product => product.DiscountId != null, () =>
            {
                RuleFor(product => product.DiscountId)
                .MustAsync(async (id, cancellationToken) => await _discountReadRepository.IsExistAsync(c => c.Id == id, cancellationToken))
                .WithMessage("Discount must be defined");
            });

            // Validation for product images
            RuleForEach(p => p.ProductImages)
                .NotNull()
                    .WithMessage("Each product image is required.")
                .Must(file => file.IsSizeOk(ProductConst.MaxFileSizeInMb))
                    .WithMessage($"File size should not exceed {ProductConst.MaxFileSizeInMb} MB.")
                .Must(file => file.RestrictExtension([".jpg", ".png", ".gif"]))
                    .WithMessage("Only .jpg, .png, and .gif files are allowed.")
                .Must(file => file.RestrictMimeTypes(["image/jpeg", "image/png", "image/gif"]))
                    .WithMessage("Only image files (JPEG, PNG, GIF) are allowed.");
        }
    }
}
