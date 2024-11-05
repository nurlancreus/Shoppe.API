using Microsoft.AspNetCore.Http;
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
        Task<GetBlogDTO> GetAsync(Guid blogId, CancellationToken cancellationToken);
        Task<GetAllBlogsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task CreateAsync(CreateBlogDTO createBlogDTO, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateBlogDTO updateBlogDTO, CancellationToken cancellationToken);
        Task DeleteAsync(Guid blogId, CancellationToken cancellationToken);
        Task ChangeCoverImageAsync(Guid blogId, Guid? newCoverImageId, IFormFile? newCoverImageFile, CancellationToken cancellationToken);
        Task RemoveImageAsync(Guid blogId, Guid imageId, CancellationToken cancellationToken);
        Task<List<GetImageFileDTO>> GetAllBlogImagesAsync(CancellationToken cancellationToken, Guid? blogId = null);
       // Task<List<GetReplyDTO>> GetRepliesByBlogAsync(string blogId, CancellationToken cancellationToken);
    }
}
