using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.DTOs.Review;
using Shoppe.Domain.Entities;
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
                Id = category.Id.ToString(),
                Name = category.Name,
                Description = category.Description,
                Type = category.Discriminator,
            };
        }

        public static GetReviewDTO ToGetReviewDTO(this Review review)
        {
            return new GetReviewDTO()
            {
                Id = review.Id.ToString(),
                FirstName = review.FirstName!,
                LastName = review.LastName!,
                Rating = (int)review.Rating,
                Body = review.Body,
                CreatedAt = review.CreatedAt,
            };
        }

        public static GetContactDTO ToGetContactDTO(this Contact contact)
        {
            return new GetContactDTO()
            {
                Id = contact.Id.ToString(),
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
                CreatedAt = contact.CreatedAt
            };
        }
    }
}
