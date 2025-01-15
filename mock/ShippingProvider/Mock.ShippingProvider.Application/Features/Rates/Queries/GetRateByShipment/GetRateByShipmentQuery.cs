using MediatR;
using Mock.ShippingProvider.Application.Features.Rates.DTOs;
using Mock.ShippingProvider.Application.Responses;
using Mock.ShippingProvider.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.Rates.Queries.GetRateByShipment
{
    public record GetRateByShipmentQuery : IRequest<ResponseWithData<ShippingRateDTO>>
    {
        public Guid ShipmentId { get; set; }
    }
}
