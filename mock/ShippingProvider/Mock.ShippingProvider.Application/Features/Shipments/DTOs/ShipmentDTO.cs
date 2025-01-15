using Mock.ShippingProvider.Application.Features.Addresses.DTOs;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Features.Rates.DTOs;
using Mock.ShippingProvider.Domain.Entities;
using Mock.ShippingProvider.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.Shipments.DTOs
{
    public record ShipmentDTO
    {
        public Guid Id { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;  // Unique tracking number
        public ShippingStatus Status { get; set; }  // Using enum for status
        public ShippingRateDTO? Rate { get; set; }
        public DateTime EstimatedDate { get; set; }

        public AddressDTO? OriginAddress { get; set; } // Navigation property to Origin Address if null then origin address taken from client (company)
        public AddressDTO? DestinationAddress { get; set; } // Navigation property to Destination Address

        public ApiClientDTO? ApiClient { get; set; } // Navigation property to ApiClient
        public DateTime CreatedAt { get; set; }

        public ShipmentDTO() { }
        public ShipmentDTO(Shipment shipment)
        {
            Id = shipment.Id;
            TrackingNumber = shipment.TrackingNumber;
            Status = shipment.Status;
            Rate = new ShippingRateDTO(shipment.Rate);
            EstimatedDate = shipment.EstimatedDate;
            OriginAddress = shipment.OriginAddress is not null ? new AddressDTO(shipment.OriginAddress) : null;
            DestinationAddress = new AddressDTO(shipment.DestinationAddress);
            ApiClient = new ApiClientDTO(shipment.ApiClient);
            CreatedAt = shipment.CreatedAt;
        }
    }
}
