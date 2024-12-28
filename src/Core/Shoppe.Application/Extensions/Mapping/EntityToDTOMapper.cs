using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.DTOs.Address;
using Shoppe.Application.DTOs.Basket;
using Shoppe.Application.DTOs.Blog;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.DTOs.Coupon;
using Shoppe.Application.DTOs.Discount;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Order;
using Shoppe.Application.DTOs.Product;
using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.DTOs.User;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Contacts;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Extensions.Mapping
{
    public static class EntityToDTOMapper
    {
        public static GetCategoryDTO ToGetCategoryDTO(this Category category)
        {
            return new GetCategoryDTO
            {
                Id = category.Id,
                DiscountId = ((category as ProductCategory)?.Discount?.IsActive ?? false) ? (category as ProductCategory)?.DiscountId : null,
                Name = category.Name,
                Description = category.Description,
                Type = category.Type,
                CreatedAt = category.CreatedAt,
            };
        }

        public static GetTagDTO ToGetTagDTO(this Tag tag)
        {
            return new GetTagDTO
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                Type = tag.Type,
                CreatedAt = tag.CreatedAt,
            };
        }

        public static GetBlogDTO ToGetBlogDTO(this Blog blog, IReactionService reactionService)
        {
            return new GetBlogDTO
            {
                Id = blog.Id,
                CoverImage = new GetImageFileDTO
                {
                    Id = blog.BlogCoverImageFile.Id,
                    FileName = blog.BlogCoverImageFile.FileName,
                    PathName = blog.BlogCoverImageFile.PathName,
                    CreatedAt = blog.BlogCoverImageFile.CreatedAt
                },
                Author = new GetUserDTO
                {
                    Id = blog.Author.Id,
                    FirstName = blog.Author.FirstName!,
                    LastName = blog.Author.LastName!,
                    Email = blog.Author.Email!,
                    UserName = blog.Author.UserName!,
                    CreatedAt = blog.Author.CreatedAt
                },
                Title = blog.Title,
                Content = blog.Content,
                ContentImages = blog.ContentImages.Select(i => i.ToGetContentFileDTO()).ToList(),
                Categories = blog.Categories.Select(c => c.ToGetCategoryDTO()).ToList(),
                Tags = blog.Tags.Select(t => t.ToGetTagDTO()).ToList(),
                Reactions = reactionService.GetBlogReactions(blog),
                CreatedAt = blog.CreatedAt
            };
        }

        public static GetOrderDTO ToGetOrderDTO(this Order order, ICalculatorService calculatorService)
        {
            return new GetOrderDTO
            {
                Id = order.Id,
                OrderStatus = order.OrderStatus.ToString(),
                OrderCode = order.OrderCode,
                ShippingCost = calculatorService.CalculateShippingCost(0), // modify distance
                Total = calculatorService.CalculateCouponAppliedPrice(order),
                SubTotal = calculatorService.CalculateTotalBasketItemsPrice(order.Basket),
                CreatedAt = order.CreatedAt
            };
        }

        public static GetBasketDTO ToGetBasketDTO(this Basket basket, ICalculatorService calculatorService)
        {
            return new GetBasketDTO()
            {
                Id = basket.Id,
                CreatedAt = basket.CreatedAt,
                Items = basket.Items.Select(bi => bi.ToGetBasketItemDTO(calculatorService)
                ).ToList(),
                User = new GetUserDTO
                {
                    Id = basket.User.Id,
                    FirstName = basket.User.FirstName!,
                    LastName = basket.User.LastName!,
                    Email = basket.User.Email!,
                    UserName = basket.User.UserName!,
                    CreatedAt = basket.User.CreatedAt,
                },
                TotalAmount = calculatorService.CalculateTotalBasketItemsPrice(basket),
                TotalDiscountedAmount = calculatorService.CalculateTotalDiscountedBasketItemsPrice(basket)
            };
        }

        public static GetBasketItemDTO ToGetBasketItemDTO(this BasketItem basketItem, IDiscountCalculatorService discountCalculatorService)
        {
            var (discountedPrice, generalDiscountPercentage) = discountCalculatorService.CalculateDiscountedPrice(basketItem.Product);


            return new GetBasketItemDTO
            {
                Id = basketItem.Id.ToString(),
                ProductName = basketItem.Product.Name,
                Price = basketItem.Product.Price,
                Quantity = basketItem.Quantity,
                ProductStock = basketItem.Product.Stock,
                Colors = basketItem.Product.ProductDetails.Colors.Select(c => c.ToString()).ToList(),
                Image = basketItem.Product.ProductImageFiles.Where(i => i.IsMain).Select(i => i.ToGetImageFileDTO()).FirstOrDefault()!,
                DiscountedPrice = discountedPrice,
                TotalPrice = basketItem.Quantity * basketItem.Product.Price,
                TotalDiscountedPrice = discountedPrice != null ? basketItem.Quantity * (discountedPrice) : null,
                CreatedAt = basketItem.CreatedAt,
            };
        }

        public static GetAddressDTO ToGetBillingAddressDTO(this BillingAddress address)
        {
            return new GetAddressDTO
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Email = address.Email,
                Phone = address.Phone,
                Country = address.Country,
                City = address.City,
                PostalCode = address.PostalCode,
                StreetAddress = address.StreetAddress
            };
        }

        public static GetAddressDTO ToGetShippingAddressDTO(this ShippingAddress address)
        {
            return new GetAddressDTO
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Email = address.Email,
                Phone = address.Phone,
                Country = address.Country,
                City = address.City,
                PostalCode = address.PostalCode,
                StreetAddress = address.StreetAddress
            };
        }

        public static GetProductDTO ToGetProductDTO(this Product product, ICalculatorService calculatorService)
        {
            var (discountedPrice, generalDiscountPercentage) = calculatorService.CalculateDiscountedPrice(product);

            return new GetProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Info = product.Info,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                DiscountId = (product.Discount?.IsActive ?? false) ? product.DiscountId : null,
                Weight = product.ProductDetails.Weight,
                Height = product.ProductDetails.Height,
                Width = product.ProductDetails.Width,
                Colors = product.ProductDetails.Colors.Select(c => c.ToString()).ToList(),
                Materials = product.ProductDetails.Materials.Select(m => m.ToString()).ToList(),
                Categories = product.Categories.Select(c => c.ToGetCategoryDTO()).ToList(),
                ProductImages = product.ProductImageFiles.Select(i => i.ToGetImageFileDTO()).ToList(),
                DiscountPercentage = generalDiscountPercentage,
                DiscountedPrice = discountedPrice,
                Rating = calculatorService.CalculateAvgRating(product.Reviews.Cast<Review>().ToList()),
                CreatedAt = product.CreatedAt
            };
        }

        public static GetImageFileDTO ToGetImageFileDTO(this ImageFile imageFile)
        {
            return new GetImageFileDTO
            {
                Id = imageFile.Id,
                FileName = imageFile.FileName,
                PathName = imageFile.PathName,
                IsMain = imageFile.IsMain,
                CreatedAt = imageFile.CreatedAt,
            };
        }

        public static GetContentFileDTO ToGetContentFileDTO(this ContentImageFile contentImageFile)
        {
            return new GetContentFileDTO
            {
                Id = contentImageFile.Id,
                FileName = contentImageFile.FileName,
                PathName = contentImageFile.PathName,
                PreviewUrl = contentImageFile.PreviewUrl,
                CreatedAt = contentImageFile.CreatedAt,
            };
        }

        public static GetUserDTO ToGetUserDTO(this ApplicationUser user)
        {
            return new GetUserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                UserName = user.UserName!,
                Email = user.Email!,
                Phone = user.PhoneNumber!,
                IsActive = user.IsActive,
                ProfilePictures = user.ProfilePictureFiles.Select(ToGetImageFileDTO).ToList(),
                ProfilePicture = user.ProfilePictureFiles.Where(i => i.IsMain).Select(i => i.ToGetImageFileDTO()).FirstOrDefault(),
                CreatedAt = user.CreatedAt,
            };
        }

        public static GetReviewDTO ToGetReviewDTO(this Review review)
        {
            var profilePic = review.Reviewer.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);

            return new GetReviewDTO()
            {
                Id = review.Id,
                AuthorId = review.ReviewerId,
                FirstName = review.Reviewer?.FirstName!,
                LastName = review.Reviewer?.LastName!,
                Rating = (int)review.Rating,
                ProfilePhoto = profilePic != null ? new GetImageFileDTO
                {
                    Id = profilePic.Id,
                    FileName = profilePic.FileName,
                    PathName = profilePic.PathName,
                    CreatedAt = profilePic.CreatedAt,
                } : null,
                Body = review.Body,
                CreatedAt = review.CreatedAt,
            };
        }

        public static GetReplyDTO ToGetReplyDTO(this Reply reply, IReactionService reactionService)
        {
            var profilePic = reply.Replier.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);

            return new GetReplyDTO
            {
                Id = reply.Id,
                AuthorId = reply.ReplierId,
                FirstName = reply.Replier.FirstName!,
                LastName = reply.Replier.LastName!,
                ProfilePhoto = profilePic != null ? new GetImageFileDTO
                {
                    IsMain = profilePic.IsMain,
                    Id = profilePic.Id,
                    FileName = profilePic.FileName,
                    PathName = profilePic.PathName,
                    CreatedAt = profilePic.CreatedAt,
                } : null,
                Body = reply.Body,
                Depth = reply.Depth,
                Replies = reply.Children.Select(r => r.ToGetReplyDTO(reactionService)).ToList(),
                CreatedAt = reply.CreatedAt,
                Reactions = reactionService.GetReplyReactions(reply)
            };
        }

        public static GetDiscountDTO ToGetDiscountDTO(this Discount discount)
        {
            return new GetDiscountDTO
            {
                Id = discount.Id,
                Name = discount.Name,
                Description = discount.Description,
                DiscountPercentage = discount.DiscountPercentage,
                IsActive = discount.IsActive,
                Products = discount.Products.Select(p => new GetProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList(),
                Categories = discount.Categories.Select(c => new GetCategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList(),
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                CreatedAt = discount.CreatedAt,
            };
        }

        public static GetCouponDTO ToGetCouponDTO(this Coupon coupon)
        {
            return new GetCouponDTO
            {
                Id = coupon.Id,
                Code = coupon.Code,
                DiscountPercentage = coupon.DiscountPercentage,
                UsageCount = coupon.UsageCount,
                MaxUsage = coupon.MaxUsage,
                MinimumOrderAmount = coupon.MinimumOrderAmount,
                IsActive = coupon.IsActive,
                StartDate = coupon.StartDate,
                EndDate = coupon.EndDate,
                CreatedAt = coupon.CreatedAt
            };
        }

        public static GetContactDTO ToGetContactDTO(this Contact contact)
        {
            var contactDTO = new GetContactDTO()
            {
                Id = contact.Id,
                Subject = StringHelpers.SplitAndJoinString(contact.Subject.ToString(), '_', ' '),
                Message = contact.Message,
                IsAnswered = contact.IsAnswered,
                CreatedAt = contact.CreatedAt
            };

            if (contact is RegisteredContact registered)
            {
                contactDTO.FirstName = registered.User.FirstName!;
                contactDTO.LastName = registered.User.LastName!;
                contactDTO.Email = registered.User.Email!;

            }
            else if (contact is UnRegisteredContact unRegistered)
            {
                contactDTO.FirstName = unRegistered.FirstName!;
                contactDTO.LastName = unRegistered.LastName!;
                contactDTO.Email = unRegistered.Email!;
            }

            return contactDTO;
        }
    }
}
