using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Pagination
{
    public record PaginatedQueryDTO<T> : IPaginationInfo where T : IBase
    {
        public IQueryable<T> PaginatedQuery { get; set; } = null!;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public void Deconstruct(out int totalItems, out int pageSize, out int page, out int totalPages, out IQueryable<T> paginatedQuery)
        {
            totalItems = TotalItems;
            pageSize = PageSize;
            page = Page;
            totalPages = TotalPages;
            paginatedQuery = PaginatedQuery;
        }
    }
}
