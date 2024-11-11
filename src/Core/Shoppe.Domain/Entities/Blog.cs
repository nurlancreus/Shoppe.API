using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; } = null!;
        // public string Body { get; set; } = null!;

        [ForeignKey(nameof(BlogCoverImageFile))]
        public Guid BlogCoverId { get; set; }
        public BlogContentImageFile BlogCoverImageFile { get; set; } = null!;

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
        public ICollection<BlogCategory> Categories { get; set; } = [];
        public ICollection<BlogTag> Tags { get; set; } = [];
        public ICollection<BlogReply> Replies { get; set; } = [];
        public ICollection<BlogReaction> Reactions { get; set; } = [];
        public ICollection<BlogContentImageFile> ContentImages { get; set; } = [];
        public string Content { get; set; } = string.Empty;
        // public ICollection<BlogSection> Sections { get; set; } = [];
       // public ICollection<BlogReview> Reviews { get; set; } = [];

    }
}
