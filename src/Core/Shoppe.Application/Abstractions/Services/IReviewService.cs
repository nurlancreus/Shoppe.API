using Shoppe.Application.DTOs.Review;
using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IReviewService
    {
        Task CreateReviewAsync(CreateReviewDTO createReviewDTO, CancellationToken cancellationToken);
        Task UpdateReviewAsync(UpdateReviewDTO updateReviewDTO, CancellationToken cancellationToken);
        Task DeleteReviewAsync(string id, CancellationToken cancellationToken);
        Task<GetAllReviewsDTO> GetAllReviewsAsync(int page, int size, CancellationToken cancellationToken);
        Task<GetReviewDTO> GetReviewAsync(string id, CancellationToken cancellationToken);

    }
}
