using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Identity;
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
    public class ReviewService : IReviewService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReviewReadRepository _reviewReadRepository;
        private readonly IReviewWriteRepository _reviewWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;

        public ReviewService(IReviewReadRepository reviewReadRepository, IReviewWriteRepository reviewWriteRepository, IUnitOfWork unitOfWork, IPaginationService paginationService, IHttpContextAccessor httpContextAccessor, IProductReadRepository productReadRepository)
        {
            _reviewReadRepository = reviewReadRepository;
            _reviewWriteRepository = reviewWriteRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _httpContextAccessor = httpContextAccessor;
            _productReadRepository = productReadRepository;
        }

        public async Task CreateReviewAsync(CreateReviewDTO createReviewDTO, CancellationToken cancellationToken)
        {

            var review = new Review();

            if (Guid.TryParse(createReviewDTO.ProductId, out Guid parsedId) && await _productReadRepository.IsExist(p => p.Id == parsedId, cancellationToken))
            {
                review.ProductId = parsedId;
            }
            else
            {
                throw new InvalidIdException();
            }

            var user = _httpContextAccessor.HttpContext?.User;


            if (user != null && (user.Identity?.IsAuthenticated ?? false))
            {
                // Assuming first name and last name are stored as claims
                review.FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value;
                review.LastName = user.FindFirst(ClaimTypes.Surname)?.Value;
                review.Email = user.FindFirst(ClaimTypes.Email)?.Value;
                review.ApplicationUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            else
            {
                review.FirstName = createReviewDTO.FirstName;
                review.LastName = createReviewDTO.LastName;
                review.Email = createReviewDTO.Email;
                //review.SaveMe = createReviewDTO.SaveMe;

                if (createReviewDTO.SaveMe == true)
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(ReviewConst.SavedReviewsExpireDate),
                        Secure = true  
                    };

                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("FirstName", createReviewDTO.FirstName!, cookieOptions);
                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("LastName", createReviewDTO.LastName!, cookieOptions);
                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("Email", createReviewDTO.Email!, cookieOptions);
                }
            }

            review.Rating = (Rating)createReviewDTO.Rating;

            await _reviewWriteRepository.AddAsync(review, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteReviewAsync(string id, CancellationToken cancellationToken)
        {
            var review = await _reviewReadRepository.GetByIdAsync(id, cancellationToken);

            if (review == null)
            {
                throw new EntityNotFoundException(nameof(cancellationToken));
            }

            bool isDeleted = _reviewWriteRepository.Delete(review);

            if (!isDeleted)
            {
                throw new DeleteNotSucceedException(nameof(review));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetAllReviewsDTO> GetAllReviewsAsync(int page, int size, CancellationToken cancellationToken)
        {
            var query = await _reviewReadRepository.GetAllAsync(false);

            var (totalItems, _pageSize, _page, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(page, size, query);

            var reviews = await paginatedQuery.Select(r => r.ToGetReviewDTO()).ToListAsync(cancellationToken);

            return new GetAllReviewsDTO()
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
            {
                throw new EntityNotFoundException(nameof(cancellationToken));
            }

            return review.ToGetReviewDTO();
        }

        public async Task UpdateReviewAsync(UpdateReviewDTO updateReviewDTO, CancellationToken cancellationToken)
        {
            var review = await _reviewReadRepository.GetByIdAsync(updateReviewDTO.Id, cancellationToken);

            if (review == null)
            {
                throw new EntityNotFoundException(nameof(review));
            }

            if (!string.IsNullOrEmpty(updateReviewDTO.Body))
            {
                review.Body = updateReviewDTO.Body;
            }

            if (updateReviewDTO.Rating != null)
            {
                review.Rating = (Rating)updateReviewDTO.Rating;
            }

            bool isUpdated = _reviewWriteRepository.Update(review);

            if (!isUpdated)
            {
                throw new UpdateNotSucceedException(nameof(review));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
