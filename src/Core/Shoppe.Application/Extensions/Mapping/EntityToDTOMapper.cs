using Shoppe.Application.DTOs.Blog;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.DTOs.User;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
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

        public static GetBlogDTO ToGetBlogDTO(this Blog blog)
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
                CreatedAt = blog.CreatedAt
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
            return new GetReviewDTO()
            {
                Id = review.Id,
                FirstName = review.Reviewer?.FirstName!,
                LastName = review.Reviewer?.LastName!,
                Rating = (int)review.Rating,
                Body = review.Body,
                CreatedAt = review.CreatedAt,
            };
        }

        public static GetReplyDTO ToGetReplyDTO(this Reply reply)
        {
            var profilePic = reply.Replier.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);

            return new GetReplyDTO
            {
                Id = reply.Id,
                FirstName = reply.Replier.FirstName!,
                LastName = reply.Replier.LastName!,
                ProfilePhoto = profilePic != null ? new GetImageFileDTO
                {
                    Id = profilePic.Id,
                    FileName = profilePic.FileName,
                    PathName = profilePic.PathName,
                    CreatedAt = profilePic.CreatedAt,
                } : null,
                Body = reply.Body,
                Type = reply.Type,
                Replies = reply.Replies.Select(r => r.ToGetReplyDTO()).ToList(),
                CreatedAt = reply.CreatedAt
            };
        }

        public static GetContactDTO ToGetContactDTO(this Contact contact)
        {
            return new GetContactDTO()
            {
                Id = contact.Id,
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
