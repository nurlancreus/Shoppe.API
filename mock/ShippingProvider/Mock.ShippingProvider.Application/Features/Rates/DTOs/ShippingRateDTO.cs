using Mock.ShippingProvider.Application.Features.Shipments.DTOs;
using Mock.ShippingProvider.Domain.Entities;
using Mock.ShippingProvider.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.Rates.DTOs
{
    public record ShippingRateDTO
    {
        public Guid Id { get; set; }
        public decimal Rate { get; set; } 
        public ShippingMethod Method { get; set; } 
        public double Weight { get; set; } 
        public string Dimensions { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ShipmentDTO? Shipment { get; set; }
        public ShippingRateDTO() { }

        public ShippingRateDTO(ShippingRate shippingRate)
        {
            Id = shippingRate.Id;
            Rate = shippingRate.Rate;
            Method = shippingRate.Method;
            Weight = shippingRate.Weight;
            Dimensions = shippingRate.Dimensions;
            CreatedAt = shippingRate.CreatedAt;
            Shipment = new ShipmentDTO(shippingRate.Shipment);
        }
    }
}
