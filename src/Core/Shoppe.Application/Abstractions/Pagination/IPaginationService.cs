using Shoppe.Application.DTOs.Pagination;
using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Shoppe.Application.Abstractions.Pagination
{
    public interface IPaginationService
    {
        Task<PaginatedQueryDTO<T>> ConfigurePaginationAsync<T>(int page, int pageSize, IQueryable<T> entities) where T : BaseEntity;
    }
}
