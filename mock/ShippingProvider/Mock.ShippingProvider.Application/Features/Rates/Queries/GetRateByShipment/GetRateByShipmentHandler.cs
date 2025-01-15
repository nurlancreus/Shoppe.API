using MediatR;
using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Application.Features.Rates.DTOs;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.Rates.Queries.GetRateByShipment
{
    public class GetRateByShipmentHandler : IRequestHandler<GetRateByShipmentQuery, ResponseWithData<ShippingRateDTO>>
    {
        private readonly IShipmentRepository _shipmentRepository;

        public GetRateByShipmentHandler(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public async Task<ResponseWithData<ShippingRateDTO>> Handle(GetRateByShipmentQuery request, CancellationToken cancellationToken)
        {
            var shippingRate = await _shipmentRepository.Table
                .Include(s => s.Rate)
                .Select(s => s.Rate)
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

            if (shippingRate is null)
            {
                return new ResponseWithData<ShippingRateDTO>
                {
                    IsSuccess = false,
                    Message = "Shipping rate not found"
                };
            }

            var dto = new ShippingRateDTO(shippingRate);

            return new ResponseWithData<ShippingRateDTO>
            {
                IsSuccess = true,
                Message = "Shipping rate retrieved",
                Data = dto
            };
        }
    }
}
