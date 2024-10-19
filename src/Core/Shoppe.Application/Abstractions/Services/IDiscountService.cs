using Shoppe.Application.DTOs.Discount;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IDiscountService
    {
        Task CreateAsync(CreateDiscountDTO createDiscountDTO, CancellationToken cancellationToken);
        Task<GetDiscountDTO> GetAsync(string id, CancellationToken cancellationToken);
        Task<GetAllDiscountsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateDiscountDTO updateDiscountDTO, CancellationToken cancellationToken);
        Task DeleteAsync(string id, CancellationToken cancellationToken);
        Task AssignDiscountAsync(string entityId, string discountId, EntityType entityType, CancellationToken cancellationToken, bool update = false);
        Task AssignDiscountAsync(IDiscountable entity, string discountId, CancellationToken cancellationToken, bool update = false);

    }
}
