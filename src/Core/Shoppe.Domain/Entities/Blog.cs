﻿using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
        public ICollection<BlogImageFile> BlogImageFiles { get; set; } = [];
        public ICollection<Category> Categories { get; set; } = [];

    }
}
