using Shoppe.Application.DTOs.Coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface ICouponService
    {
        Task<GetAllCouponsDTO> GetAll(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<GetCouponDTO> Get(Guid id, CancellationToken cancellationToken = default);
        Task CreateAsync (CreateCouponDTO createCouponDTO, CancellationToken cancellationToken = default);
        Task UpdateAsync (UpdateCouponDTO updateCouponDTO, CancellationToken cancellationToken = default);
        Task DeleteAsync (Guid id, CancellationToken cancellationToken = default);
        Task ToggleAsync (Guid id, CancellationToken cancellationToken = default);
    }
}
