using Shoppe.Application.DTOs.Blog;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.DTOs.Review;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IBlogService
    {
        Task<GetBlogDTO> GetAsync(string blogId, CancellationToken cancellationToken);
        Task<GetAllBlogsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task CreateAsync(CreateBlogDTO createBlogDTO, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateBlogDTO updateBlogDTO, CancellationToken cancellationToken);
        Task DeleteAsync(string blogId, CancellationToken cancellationToken);
        Task ChangeCoverImageAsync(string blogId, string newCoverImageId, CancellationToken cancellationToken);
        Task RemoveImageAsync(string blogId, string imageId, CancellationToken cancellationToken);
        Task<List<GetImageFileDTO>> GetAllBlogImagesAsync(CancellationToken cancellationToken, string? blogId = null);
       // Task<List<GetReplyDTO>> GetRepliesByBlogAsync(string blogId, CancellationToken cancellationToken);
    }
}
