using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Order.GetAll
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetAllOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllOrdersQueryResponse
            {
                IsSuccess = true,
                Page = result.Page,
                PageSize = result.PageSize,
                Data = result.Orders,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages
            };
        }
    }
}
