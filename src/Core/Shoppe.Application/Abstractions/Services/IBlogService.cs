using Shoppe.Application.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IBlogService
    {
        Task<GetBlogDTO> GetAsync (string blogId, CancellationToken cancellationToken);
        Task<GetAllBlogsDTO> GetAllAsync (int page, int pageSize, CancellationToken cancellationToken);
        Task CreateAsync (CreateBlogDTO createBlogDTO, CancellationToken cancellationToken);
        Task UpdateAsync (UpdateBlogDTO updateBlogDTO, CancellationToken cancellationToken);
        Task DeleteAsync (string blogId, CancellationToken cancellationToken);
        Task ChangeMainImageAsync(string blogId, string newMainImageId, CancellationToken cancellationToken);
        Task RemoveImageAsync(string blogId, string newMainImageId, CancellationToken cancellationToken);
    }
}
