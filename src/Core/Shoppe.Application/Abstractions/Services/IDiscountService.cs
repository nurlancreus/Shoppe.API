using Shoppe.Application.DTOs.Discount;
using Shoppe.Domain.Entities;
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
        Task CreateAsync(CreateDiscountDTO createDiscountDTO, CancellationToken cancellationToken =default);
        Task ToggleDiscountAsync (Guid discountId, CancellationToken cancellationToken = default);
        Task<GetDiscountDTO> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GetAllDiscountsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateDiscountDTO updateDiscountDTO, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task AssignDiscountAsync(Guid entityId, Guid discountId, EntityType entityType, CancellationToken cancellationToken = default, bool update = false);
        Task AssignDiscountAsync(IDiscountable entity, Guid discountId, CancellationToken cancellationToken = default, bool update = false);

    }
}
