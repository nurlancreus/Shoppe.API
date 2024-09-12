﻿using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.DTOs.Pagination;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Exceptions;


namespace Shoppe.Infrastructure.Concretes.Services
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginatedQueryDTO<T>> ConfigurePaginationAsync<T>(int page, int pageSize, IQueryable<T> entities) where T : BaseEntity
        {
            // Validate PageSize
            if (pageSize <= 0)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageSize, pageSize);
            }

            // Get total items
            var totalItems = await entities.CountAsync();
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

            // Return Pagination response
            var response = new PaginatedQueryDTO<T>()
            {
                Page = page,
                PageSize =  pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PaginatedQuery = paginatedQuery
            };

            return response;
        }
    }
}