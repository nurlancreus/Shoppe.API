using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.DTOs.Pagination;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginatedQueryDTO<T>> ConfigurePaginationAsync<T>(int page, int pageSize, IQueryable<T> entities, CancellationToken cancellationToken) where T : IBase
        {
            // Handle the case where both page and pageSize are -1
            if (page == -1 && pageSize == -1)
            {
                // Return all entities without pagination
                var total = await entities.CountAsync(cancellationToken: cancellationToken);

                return new PaginatedQueryDTO<T>()
                {
                    Page = 1,  // Set Page to 1 (since we are returning everything)
                    PageSize = total,  // PageSize is set to totalItems as we're returning all items
                    TotalItems = total,
                    TotalPages = 1,  // Only 1 "page" as everything is returned
                    PaginatedQuery = entities  // No pagination, return full query
                };
            }

            // Validate PageSize
            if (pageSize <= 0)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageSize, pageSize);
            }

            // Get total items count
            var totalItems = await entities.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Adjust totalPages to be at least 1
            totalPages = totalPages == 0 ? 1 : totalPages;

            // Validate PageNumber
            if (page < 1 || page > totalPages)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageNumber, page);
            }

            // Calculate pagination
            var skip = (page - 1) * pageSize;
            var paginatedQuery = entities
                .Skip(skip)
                .Take(pageSize);

            // Return pagination response
            return new PaginatedQueryDTO<T>()
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PaginatedQuery = paginatedQuery
            };
        }
    }
}
