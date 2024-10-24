﻿using MediatR;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Section;
using Shoppe.Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Create
{
    public class CreateBlogCommandRequest : IRequest<CreateBlogCommandResponse>
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = [];
        public List<CreateSectionDTO> Sections { get; set; } = [];

    }
}
