using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.Extensions.Helpers;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    namespace Shoppe.Persistence.Concretes.Services
    {
        public class ReviewService : IReviewService
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IReviewReadRepository _reviewReadRepository;
            private readonly IReviewWriteRepository _reviewWriteRepository;
            private readonly IProductReadRepository _productReadRepository;
            private readonly IBlogReadRepository _blogReadRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPaginationService _paginationService;

            public ReviewService(IReviewReadRepository reviewReadRepository, IReviewWriteRepository reviewWriteRepository, IUnitOfWork unitOfWork, IPaginationService paginationService,
              IHttpContextAccessor httpContextAccessor, IProductReadRepository productReadRepository,
              IBlogReadRepository blogReadRepository)
            {
                _reviewReadRepository = reviewReadRepository;
                _reviewWriteRepository = reviewWriteRepository;
                _unitOfWork = unitOfWork;
                _paginationService = paginationService;
                _httpContextAccessor = httpContextAccessor;
                _productReadRepository = productReadRepository;
                _blogReadRepository = blogReadRepository;
            }

            public async Task CreateReviewAsync(CreateReviewDTO createReviewDTO, CancellationToken cancellationToken)
            {
                var user = ContextHelpers.GetCurrentUser(_httpContextAccessor);

                Review? review = await ValidateAndCreateReviewAsync(createReviewDTO, cancellationToken);

                if (review == null) throw new InvalidOperationException("Review cannot be created");

                review.ApplicationUserId = user.Id;
                review.Body = createReviewDTO.Body;

                review.Rating = (Rating)Math.Clamp(createReviewDTO.Rating, 1, 5);

                await _reviewWriteRepository.AddAsync(review, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            private async Task<Review?> ValidateAndCreateReviewAsync(CreateReviewDTO createReviewDTO, CancellationToken cancellationToken)
            {
                if (createReviewDTO.ReviewType == ReviewType.Product)
                {
                    bool isExist = await _productReadRepository.IsExistAsync(p => p.Id.ToString() == createReviewDTO.EntityId, cancellationToken);
                    if (!isExist) throw new AddNotSucceedException("Product does not exist");

                    return new ProductReview { ProductId = Guid.Parse(createReviewDTO.EntityId) };
                }
                //else if (createReviewDTO.ReviewType == ReviewType.Blog)
                //{
                //    bool isExist = await _blogReadRepository.IsExist(b => b.Id.ToString() == createReviewDTO.EntityId, cancellationToken);
                //    if (!isExist) throw new AddNotSucceedException("Blog does not exist");

                //    return new BlogReview { BlogId = Guid.Parse(createReviewDTO.EntityId) };
                //}

                return null;
            }

            public async Task DeleteReviewAsync(string id, CancellationToken cancellationToken)
            {
                var user = ContextHelpers.GetCurrentUser(_httpContextAccessor);
                var review = await _reviewReadRepository.GetByIdAsync(id, cancellationToken);

                if (review == null)
                    throw new EntityNotFoundException(nameof(review));

                if (CanModifyReview(user, review))
                {
                    bool isDeleted = _reviewWriteRepository.Delete(review);

                    if (!isDeleted)
                        throw new DeleteNotSucceedException("Failed to delete the review.");

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw new UnauthorizedAccessException("You do not have permission to delete this review.");
                }
            }

            public async Task<GetAllReviewsDTO> GetAllReviewsAsync(int page, int size, CancellationToken cancellationToken)
            {
                var query = _reviewReadRepository.Table.Include(r => r.Reviewer).AsNoTrackingWithIdentityResolution().AsQueryable();

                var (totalItems, _pageSize, _page, totalPages, paginatedQuery) =
                    await _paginationService.ConfigurePaginationAsync(page, size, query);

                var reviews = await paginatedQuery
                    .Select(r => r.ToGetReviewDTO())
                    .ToListAsync(cancellationToken);

                return new GetAllReviewsDTO
                {
                    Page = page,
                    PageSize = size,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Reviews = reviews
                };
            }

            public async Task<GetReviewDTO> GetReviewAsync(string id, CancellationToken cancellationToken)
            {
                var review = await _reviewReadRepository.GetByIdAsync(id, cancellationToken, false);

                if (review == null)
                    throw new EntityNotFoundException(nameof(review));

                await _reviewReadRepository.Table.Entry(review).Reference(r => r.Reviewer).LoadAsync(cancellationToken);

                return review.ToGetReviewDTO();
            }

            public async Task UpdateReviewAsync(UpdateReviewDTO updateReviewDTO, CancellationToken cancellationToken)
            {
                var user = ContextHelpers.GetCurrentUser(_httpContextAccessor);
                var review = await _reviewReadRepository.GetByIdAsync(updateReviewDTO.Id, cancellationToken);

                if (review == null)
                    throw new EntityNotFoundException(nameof(review));

                if (CanModifyReview(user, review))
                {
                    if (!string.IsNullOrEmpty(updateReviewDTO.Body))
                        review.Body = updateReviewDTO.Body;

                    if (updateReviewDTO.Rating is int rating)
                    {
                        review.Rating = (Rating)Math.Clamp(rating, 1, 5);
                    }

                    bool isUpdated = _reviewWriteRepository.Update(review);

                    if (!isUpdated)
                        throw new UpdateNotSucceedException("Failed to update the review.");

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw new UnauthorizedAccessException("You do not have permission to update this review.");
                }
            }

            private bool CanModifyReview(ApplicationUser user, Review review)
            {
                return review.ApplicationUserId == user.Id || ContextHelpers.IsAdmin(_httpContextAccessor);
            }
        }
    }

}
