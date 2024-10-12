using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Product;
using Shoppe.Application.Features.Command.Product.UpdateProduct;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Enums;
using System.Linq;

namespace Shoppe.Application.Validators.Product
{
    public class UpdateProductCommandRequestValidator : AbstractValidator<UpdateProductCommandRequest>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public UpdateProductCommandRequestValidator(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;

            // Validate Id
            RuleFor(product => product.Id)
                .NotEmpty()
                .WithMessage("Product ID is required.");

            // Validate Name (Optional, but if provided, it must follow the rules)
            When(product => !string.IsNullOrWhiteSpace(product.Name), () =>
            {
                RuleFor(product => product.Name)
                    .MaximumLength(ProductConst.MaxNameLength)
                    .WithMessage($"Name must be less than {ProductConst.MaxNameLength} characters.");
            });

            // Validate Description (Optional, but if provided, it must follow the rules)
            When(product => !string.IsNullOrWhiteSpace(product.Description), () =>
            {
                RuleFor(product => product.Description)
                    .MaximumLength(ProductConst.MaxDescLength)
                    .WithMessage($"Description must be less than {ProductConst.MaxDescLength} characters.");
            });

            // Validate Price (Optional, but if provided, it must be greater than zero)
            When(product => product.Price.HasValue, () =>
            {
                RuleFor(product => product.Price.Value)
                    .GreaterThan(0)
                    .WithMessage("Price must be greater than zero.");
            });

            // Validate Stock (Optional, but if provided, it must be non-negative)
            When(product => product.Stock.HasValue, () =>
            {
                RuleFor(product => product.Stock.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Stock cannot be negative.");
            });

            // Validate Weigth, Height, and Width (Optional, but if provided, must be greater than zero)
            When(product => product.Weigth.HasValue, () =>
            {
                RuleFor(product => product.Weigth.Value)
                    .GreaterThan(0)
                    .WithMessage("Weight must be greater than zero.");
            });

            When(product => product.Height.HasValue, () =>
            {
                RuleFor(product => product.Height.Value)
                    .GreaterThan(0)
                    .WithMessage("Height must be greater than zero.");
            });

            When(product => product.Width.HasValue, () =>
            {
                RuleFor(product => product.Width.Value)
                    .GreaterThan(0)
                    .WithMessage("Width must be greater than zero.");
            });

            // Validate Material using EnumHelper (Optional, but if provided, must be valid)
            When(product => product.Materials != null && product.Materials.Any(), () =>
            {
                RuleForEach(product => product.Materials)
                    .Must(material => EnumHelpers.IsDefinedEnum<Material>(material, out _))
                    .WithMessage("Material must be a valid enum value.");
            });

            // Validate Colors: At least one valid color (Optional, but if provided, must be valid)
            When(product => product.Colors != null && product.Colors.Any(), () =>
            {
                RuleForEach(product => product.Colors)
                    .Must(color => EnumHelpers.IsDefinedEnum<Color>(color, out _))
                    .WithMessage("Color must be a valid enum value.");
            });

            // Validate Categories: Must exist in the database (Optional, but if provided, must be valid)
            When(product => product.CategoryIds.Count != 0, () =>
            {
                RuleForEach(product => product.CategoryIds)
                .MustAsync(async (id, cancellationToken) => await _categoryReadRepository.IsExist(c => c.Id.ToString() == id, cancellationToken))
                .WithMessage("Category must be defined");
            });

            // Validate ProductImages: File must meet size and format requirements (Optional, but if provided)
            When(product => product.ProductImages != null && product.ProductImages.Any(), () =>
            {
                RuleForEach(p => p.ProductImages)
                    .NotNull()
                    .WithMessage("Each product image is required.")
                    .Must(file => file.Length <= ProductConst.MaxFileSizeInMb * 1024 * 1024)
                    .WithMessage($"File size should not exceed {ProductConst.MaxFileSizeInMb} MB.")
                    .Must(file => new[] { ".jpg", ".png", ".gif" }.Contains(System.IO.Path.GetExtension(file.FileName).ToLower()))
                    .WithMessage("Only .jpg, .png, and .gif files are allowed.")
                    .Must(file => new[] { "image/jpeg", "image/png", "image/gif" }.Contains(file.ContentType))
                    .WithMessage("Only image files (JPEG, PNG, GIF) are allowed.");
            });
        }
    }
}
