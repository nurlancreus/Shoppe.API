using Shoppe.Application.DTOs.Auth;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.DTOs.Product;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.Extensions.Helpers;
using Shoppe.Application.Features.Command.Auth.Login;
using Shoppe.Application.Features.Command.Auth.Register;
using Shoppe.Application.Features.Command.Category.CreateCategory;
using Shoppe.Application.Features.Command.Category.UpdateCategory;
using Shoppe.Application.Features.Command.Contact.CreateContact;
using Shoppe.Application.Features.Command.Contact.UpdateContact;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using Shoppe.Application.Features.Command.Product.UpdateProduct;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Application.Features.Command.Review.UpdateReview;
using Shoppe.Application.Features.Query.Product.GetAllProducts;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Extensions.Mapping
{
    public static class RequestToDTOMapper
    {
        public static CreateCategoryDTO ToCreateCategoryDTO(this CreateCategoryCommandRequest request)
        {
            var categoryDTO = new CreateCategoryDTO()
            {
                Name = request.Name,
                Description = request.Description,
            };

            if (Enum.TryParse(request.Type, true, out CategoryType categoryType)) categoryDTO.Type = categoryType;

            return categoryDTO;
        }
        public static CreateProductDTO ToCreateProductDTO(this CreateProductCommandRequest request)
        {
            return new CreateProductDTO
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Weigth = request.Weigth,
                Height = request.Height,
                Width = request.Width,
                Materials = request.Materials,
                Colors = request.Colors,
                CategoryIds = request.CategoryIds,
                ProductImages = request.ProductImages,
            };
        }

        public static ProductFilterParamsDTO ToProductFilterParamsDTO(this GetAllProductsQueryRequest request)
        {
            return new ProductFilterParamsDTO
            {
                Page = request.Page,
                PageSize = request.PageSize,
                CategoryName = request.CategoryName,
                InStock = request.InStock,
                MaxPrice = request.MaxPrice,
                MinPrice = request.MinPrice,
                SortOptions = ParsingHelpers.ParseSortByQuery(request.SortBy),
            };
        }

        public static UpdateCategoryDTO ToUpdateCategoryDTO(this UpdateCategoryCommandRequest request)
        {
            var categoryDTO = new UpdateCategoryDTO
            {
                Id = request.Id!,
                Name = request.Name,
                Description = request.Description,
            };

            if (Enum.TryParse(request.Type, true, out CategoryType categoryType)) categoryDTO.Type = categoryType;

            return categoryDTO;
        }

        public static UpdateProductDTO ToUpdateProductDTO(this UpdateProductCommandRequest request)
        {
            return new UpdateProductDTO
            {
                Id = request.Id!,
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                Colors = request.Colors,
                Materials = request.Materials,
                CategoryIds = request.CategoryIds,
                Description = request.Description,
                ProductImages = request.ProductImages,
                Width = request.Width,
                Height = request.Height,
                Weigth = request.Weigth
            };
        }

        public static RegisterRequestDTO ToRegisterRequestDTO(this RegisterCommandRequest request)
        {
            return new RegisterRequestDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword
            };
        }

        public static LoginRequestDTO ToLoginRequestDTO(this LoginCommandRequest request)
        {
            return new LoginRequestDTO
            {
                Email = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe,
            };
        }

        public static CreateReviewDTO ToCreateReviewDTO(this CreateReviewCommandRequest request)
        {
            return new CreateReviewDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Body = request.Body,
                ProductId = request.ProductId,
                Rating = request.Rating,
                SaveMe = request.SaveMe
            };
        }

        public static UpdateReviewDTO ToUpdateReviewDTO(this UpdateReviewCommandRequest request)
        {
            return new UpdateReviewDTO
            {
                Id = request.Id!,
                Body = request.Body,
                Rating = request.Rating
            };
        }

        public static CreateContactDTO ToCreateContactDTO(this CreateContactCommandRequest request)
        {
            return new CreateContactDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Subject = request.Subject,
                Message = request.Message
            };
        }

        public static UpdateContactDTO ToUpdateContactDTO(this UpdateContactCommandRequest request)
        {
            return new UpdateContactDTO
            {
                Id = request.Id!,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Subject = request.Subject,
                Message = request.Message
            };
        }
    }
}
