using Shoppe.Application.DTOs.Coupon;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Coupon.GetAll
{
    public class GetAllCouponsQueryResponse : AppResponseWithPaginatedData<List<GetCouponDTO>>
    {
    }
}
