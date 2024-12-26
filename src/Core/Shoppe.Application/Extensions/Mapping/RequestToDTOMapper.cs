using Shoppe.Application.DTOs.Auth;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.DTOs.Discount;
using Shoppe.Application.DTOs.Product;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.DTOs.User;
using Shoppe.Application.Helpers;
using Shoppe.Application.Features.Command.Auth.Login;
using Shoppe.Application.Features.Command.Auth.Register;
using Shoppe.Application.Features.Command.Category.CreateCategory;
using Shoppe.Application.Features.Command.Category.UpdateCategory;
using Shoppe.Application.Features.Command.Contact.CreateContact;
using Shoppe.Application.Features.Command.Contact.UpdateContact;
using Shoppe.Application.Features.Command.Discount.CreateDiscount;
using Shoppe.Application.Features.Command.Discount.UpdateDiscount;
using Shoppe.Application.Features.Command.Product.CreateProduct;
using Shoppe.Application.Features.Command.Product.UpdateProduct;
using Shoppe.Application.Features.Command.Review.CreateReview;
using Shoppe.Application.Features.Command.Review.UpdateReview;
using Shoppe.Application.Features.Command.User.Update;
using Shoppe.Application.Features.Query.Product.GetAllProducts;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Application.DTOs.Address;
using Shoppe.Application.Features.Command.Address.Billing.Create;
using Shoppe.Application.Features.Command.Address.Shipping.Create;
using Shoppe.Application.Features.Command.Address.Billing.Update;
using Shoppe.Application.Features.Command.Address.Shipping.Update;
using Shoppe.Application.DTOs.Coupon;
using Shoppe.Application.Features.Command.Coupon.Create;
using Shoppe.Application.Features.Command.Coupon.Update;

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
                Info = request.Info,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Weight = request.Weight,
                Height = request.Height,
                Width = request.Width,
                DiscountId = request.DiscountId,
                Materials = request.Materials,
                Colors = request.Colors,
                Categories = request.Categories,
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
                Discounted = request.Discounted,
                SortOptions = ParsingHelpers.ParseSortByQuery(request.SortBy),
            };
        }

        public static UpdateCategoryDTO ToUpdateCategoryDTO(this UpdateCategoryCommandRequest request)
        {
            var categoryDTO = new UpdateCategoryDTO
            {
                Id = (Guid)request.Id!,
                Name = request.Name,
                Description = request.Description,
            };

            return categoryDTO;
        }

        public static UpdateProductDTO ToUpdateProductDTO(this UpdateProductCommandRequest request)
        {
            return new UpdateProductDTO
            {
                Id = (Guid)request.Id!,
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                Colors = request.Colors,
                Materials = request.Materials,
                Categories = request.Categories,
                Info = request.Info,
                Description = request.Description,
                ProductImages = request.ProductImages,
                Width = request.Width,
                Height = request.Height,
                Weight = request.Weight,
                DiscountId = request.DiscountId,
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

        public static UpdateReviewDTO ToUpdateReviewDTO(this UpdateReviewCommandRequest request)
        {
            return new UpdateReviewDTO
            {
                Id = (Guid)request.Id!,
                Body = request.Body,
                Rating = request.Rating
            };
        }

        public static CreateContactDTO ToCreateContactDTO(this CreateContactCommandRequest request)
        {
            var contactDTO = new CreateContactDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Message = request.Message
            };

            contactDTO.Subject = Enum.Parse<ContactSubject>(request.Subject);

            return contactDTO;
        }

        public static UpdateContactDTO ToUpdateContactDTO(this UpdateContactCommandRequest request)
        {
            var contactDTO = new UpdateContactDTO
            {
                Email = request.Email,
                Message = request.Message
            };

            if (request.Subject != null)
                contactDTO.Subject = Enum.Parse<ContactSubject>(request.Subject);

            return contactDTO;
        }

        public static CreateDiscountDTO ToCreateDiscountDTO(this CreateDiscountCommandRequest request)
        {
            return new CreateDiscountDTO
            {
                Name = request.Name,
                Description = request.Description,
                DiscountPercentage = request.DiscountPercentage,
                EndDate = request.EndDate,
                StartDate = request.StartDate
            };
        }

        public static UpdateDiscountDTO ToUpdateDiscountDTO(this UpdateDiscountCommandRequest request)
        {
            return new UpdateDiscountDTO
            {
                Id = (Guid)request.Id!,
                Name = request.Name,
                Description = request.Description,
                DiscountPercentage = request.DiscountPercentage,
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                IsActive = request.IsActive
            };
        }

        public static CreateCouponDTO ToCreateCouponDTO(this CreateCouponCommandRequest request)
        {
            return new CreateCouponDTO
            {
                Code = request.Code,
                DiscountPercentage = request.DiscountPercentage,
                MinimumOrderAmount = request.MinimumOrderAmount,
                MaxUsage = request.MaxUsage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };
        }

        public static UpdateCouponDTO ToUpdateCouponDTO(this UpdateCouponCommandRequest request)
        {
            return new UpdateCouponDTO
            {
                Id = (Guid)request.Id!,
                Code = request.Code,
                DiscountPercentage = request.DiscountPercentage,
                MinimumOrderAmount = request.MinimumOrderAmount,
                MaxUsage = request.MaxUsage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };
        }

        public static UpdateUserDTO ToUpdateUserDTO(this UpdateUserCommandRequest request)
        {
            return new UpdateUserDTO
            {
                UserId = request.UserId!,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                Phone = request.Phone,
                NewProfilePictureFile = request.NewProfilePictureFile,
                AlreadyExistingImageId = request.AlreadyExistingImageId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword,
                ConfirmNewPassword = request.ConfirmNewPassword
            };
        }

        public static CreateBillingAddressDTO ToCreateBillingAddressDTO(this CreateBillingAddressCommandRequest request)
        {
            return new CreateBillingAddressDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Country = request.Country,
                City = request.City,
                PostalCode = request.PostalCode,
                StreetAddress = request.StreetAddress
            };
        }

        public static CreateShippingAddressDTO ToCreateShippingAddressDTO(this CreateShippingAddressCommandRequest request)
        {
            return new CreateShippingAddressDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Country = request.Country,
                City = request.City,
                PostalCode = request.PostalCode,
                StreetAddress = request.StreetAddress
            };
        }

        public static UpdateBillingAddressDTO ToUpdateBillingAddressDTO(this UpdateBillingAddressCommandRequest request)
        {
            return new UpdateBillingAddressDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Country = request.Country,
                City = request.City,
                PostalCode = request.PostalCode,
                StreetAddress = request.StreetAddress
            };
        }

        public static UpdateShippingAddressDTO ToUpdateShippingAddressDTO(this UpdateShippingAddressCommandRequest request)
        {
            return new UpdateShippingAddressDTO
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Country = request.Country,
                City = request.City,
                PostalCode = request.PostalCode,
                StreetAddress = request.StreetAddress
            };
        }
    }
}
