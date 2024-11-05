using Shoppe.Application.DTOs.Review;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IReviewService
    {
        Task CreateAsync(CreateReviewDTO createReviewDTO, Guid entityId, ReviewType reviewType, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateReviewDTO updateReviewDTO, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<GetAllReviewsDTO> GetAllAsync(int page, int size, CancellationToken cancellationToken);
        Task<GetReviewDTO> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<List<GetReviewDTO>> GetReviewsByEntityAsync(Guid entityId, ReviewType reviewType, CancellationToken cancellationToken);

        Task<List<GetReviewDTO>> GetReviewsByUserAsync(string userId, CancellationToken cancellationToken);

    }
}
