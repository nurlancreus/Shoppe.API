using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.DTOs.Review;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewReadRepository _reviewReadRepository;
        private readonly IReviewWriteRepository _reviewWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IJwtSession _jwtSession;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(
            IReviewReadRepository reviewReadRepository,
            IReviewWriteRepository reviewWriteRepository,
            IUnitOfWork unitOfWork,
            IPaginationService paginationService,
            IJwtSession jwtSession,
            IProductReadRepository productReadRepository,
            IBlogReadRepository blogReadRepository,
            ILogger<ReviewService> logger)
        {
            _reviewReadRepository = reviewReadRepository;
            _reviewWriteRepository = reviewWriteRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _jwtSession = jwtSession;
            _productReadRepository = productReadRepository;
            _blogReadRepository = blogReadRepository;
            _logger = logger;
        }

        public async Task CreateAsync(CreateReviewDTO createReviewDTO, string entityId, ReviewType reviewType, CancellationToken cancellationToken)
        {
            var review = await CreateReviewByTypeAsync(entityId, reviewType, cancellationToken);
            review.Body = createReviewDTO.Body;
            review.ReviewerId = _jwtSession.GetUserId();
            review.Rating = (Rating)Math.Clamp(createReviewDTO.Rating, 1, 5);

            if (!await _reviewWriteRepository.AddAsync(review, cancellationToken))
            {
                _logger.LogWarning("Failed to add review for entity ID {EntityId}", entityId);
                throw new AddNotSucceedException("Cannot add new review");
            }

            _logger.LogInformation("Review added successfully for entity ID {EntityId}", entityId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(string reviewId, CancellationToken cancellationToken)
        {
            var review = await GetAndValidateReviewAsync(reviewId, cancellationToken);

            if (!_reviewWriteRepository.Delete(review))
            {
                _logger.LogWarning("Failed to delete review with ID {ReviewId}", reviewId);
                throw new DeleteNotSucceedException("Cannot delete the review");
            }

            _logger.LogInformation("Review deleted successfully with ID {ReviewId}", reviewId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetAllReviewsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var reviewQuery = _reviewReadRepository.Table.AsQueryable().AsNoTracking();
            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, reviewQuery, cancellationToken);
            var reviews = await paginationResult.PaginatedQuery
                .Include(r => r.Reviewer)
                    .ThenInclude(u => u.ProfilePictureFiles)
                .ToListAsync(cancellationToken);

            var reviewsDto = reviews.Select(MapReviewToDTO).ToList();

            return new GetAllReviewsDTO
            {
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages,
                Reviews = reviewsDto,
            };
        }

        public async Task<GetReviewDTO> GetAsync(string reviewId, CancellationToken cancellationToken)
        {
            var review = await _reviewReadRepository.Table
                .Include(r => r.Reviewer)
                    .ThenInclude(u => u.ProfilePictureFiles)
                .FirstOrDefaultAsync(r => r.Id.ToString() == reviewId, cancellationToken);

            if (review == null)
            {
                _logger.LogWarning("Review with ID {ReviewId} not found", reviewId);
                throw new EntityNotFoundException("Review not found");
            }

            return MapReviewToDTO(review);
        }

        public async Task<List<GetReviewDTO>> GetReviewsByEntityAsync(string entityId, ReviewType reviewType, CancellationToken cancellationToken)
        {
            var reviewQuery = reviewType switch
            {
                ReviewType.Product => _reviewReadRepository.Table.OfType<ProductReview>()
                    .Include(r => r.Reviewer)
                        .ThenInclude(u => u.ProfilePictureFiles)
                   // .Include(r => r.Product)
                    .Where(r => r.ProductId.ToString() == entityId)
                    .AsNoTracking(),

                _ => throw new InvalidOperationException("Invalid review type"),
            };

            var reviews = await reviewQuery.ToListAsync(cancellationToken);

            return reviews.Select(MapReviewToDTO).ToList();
        }

        public async Task<List<GetReviewDTO>> GetReviewsByUserAsync(string userId, CancellationToken cancellationToken)
        {
            var replies = await _reviewReadRepository.Table
                .Include(r => r.Reviewer)
                    .ThenInclude(u => u.ProfilePictureFiles)
                .Where(r => r.Reviewer.Id.ToString() == userId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return replies.Select(MapReviewToDTO).ToList();
        }

        public async Task UpdateAsync(UpdateReviewDTO updateReviewDTO, CancellationToken cancellationToken)
        {
            var review = await GetAndValidateReviewAsync(updateReviewDTO.Id, cancellationToken);

            if (!string.IsNullOrEmpty(updateReviewDTO.Body) && review.Body != updateReviewDTO.Body)
            {
                review.Body = updateReviewDTO.Body;
                _logger.LogInformation("Updated review body for review ID {ReviewId}", updateReviewDTO.Id);
            }

            if (updateReviewDTO.Rating is int rating && review.Rating != (Rating)Math.Clamp(rating, 1, 5))
            {
                review.Rating = (Rating)Math.Clamp(rating, 1, 5);
                _logger.LogInformation("Updated review rating for review ID {ReviewId}", updateReviewDTO.Id);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Review> CreateReviewByTypeAsync(string entityId, ReviewType reviewType, CancellationToken cancellationToken)
        {
            return reviewType switch
            {
                ReviewType.Product => await CreateProductReviewAsync(entityId, cancellationToken),
                _ => throw new AddNotSucceedException("Invalid review type"),
            };
        }

        private async Task<Review> CreateProductReviewAsync(string entityId, CancellationToken cancellationToken)
        {
            if (!await _productReadRepository.IsExistAsync(p => p.Id.ToString() == entityId, cancellationToken))
            {
                _logger.LogWarning("Product with ID {EntityId} not found", entityId);
                throw new EntityNotFoundException($"Product with ID {entityId} not found");
            }

            return new ProductReview { ProductId = Guid.Parse(entityId) };
        }

        private async Task<Review> GetAndValidateReviewAsync(string reviewId, CancellationToken cancellationToken)
        {
            var review = await _reviewReadRepository.Table
                .Include(r => r.Reviewer)
                    .ThenInclude(u => u.ProfilePictureFiles)
                .FirstOrDefaultAsync(r => r.Id.ToString() == reviewId, cancellationToken);

            if (review == null)
            {
                _logger.LogWarning("Review with ID {ReviewId} not found", reviewId);
                throw new EntityNotFoundException("Review not found");
            }

            if (!_jwtSession.IsAdmin() && _jwtSession.GetUserId() != review.ReviewerId)
            {
                _logger.LogWarning("Unauthorized access attempt for review ID {ReviewId}", reviewId);
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
            }

            return review;
        }

        private GetReviewDTO MapReviewToDTO(Review review)
        {
            var profilePic = review.Reviewer.ProfilePictureFiles.FirstOrDefault(p => p.IsMain);

            return new GetReviewDTO
            {
                FirstName = review.Reviewer.FirstName!,
                LastName = review.Reviewer.LastName!,
                ProfilePhoto = profilePic != null ? new GetImageFileDTO
                {
                    Id = profilePic.Id.ToString(),
                    FileName = profilePic.FileName,
                    PathName = profilePic.PathName,
                    CreatedAt = profilePic.CreatedAt,
                } : null,
                Body = review.Body,
                Rating = (int)review.Rating,
                CreatedAt = review.CreatedAt,
            };
        }
    }
}
