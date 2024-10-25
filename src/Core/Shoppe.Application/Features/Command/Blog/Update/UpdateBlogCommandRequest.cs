using MediatR;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Blog.Update
{
    public class UpdateBlogCommandRequest : IRequest<UpdateBlogCommandResponse>
    {
        public string? BlogId { get; set; }
        public string? Title { get; set; }
        public FormFile? CoverImageFile { get; set; } = null!;
        public List<string> Categories { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public List<CreateSectionDTO> Sections { get; set; } = [];
        public List<UpdateSectionDTO> UpdatedSections { get; set; } = [];
    }
}
