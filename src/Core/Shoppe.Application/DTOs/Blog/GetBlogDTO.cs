using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Content;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reaction;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.DTOs.User;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Blog
{
    public record GetBlogDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public GetImageFileDTO? CoverImage { get; set; }
        public GetUserDTO? Author { get; set; }
        public string Content { get; set; } = string.Empty;
        public ICollection<GetContentFileDTO> ContentImages { get; set; } = [];
        public List<GetCategoryDTO> Categories { get; set; } = [];
        public List<GetTagDTO> Tags { get; set; } = [];
        public List<GetReactionDTO> Reactions { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}
