using Mock.ShippingProvider.Domain.Entities.Base;
using Mock.ShippingProvider.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Domain.Entities
{
    public class Shipment : BaseEntity
    {
        public string TrackingNumber { get; set; } = string.Empty;  // Unique tracking number
        public ShippingStatus Status { get; set; }  // Using enum for status
        public ShippingRate Rate { get; set; } = null!;
        public DateTime EstimatedDate { get; set; }

        public Guid? OriginAddressId { get; set; }  // Foreign key to Address
        public Address? OriginAddress { get; set; } // Navigation property to Origin Address if null then origin address taken from client (company)
        public Guid DestinationAddressId { get; set; }  // Foreign key to Address
        public Address DestinationAddress { get; set; } = null!;  // Navigation property to Destination Address

        public Guid ApiClientId { get; set; } // Foreign key to ApiClient
        public ApiClient ApiClient { get; set; } = null!; // Navigation property to ApiClient

        private Shipment()
        {
            TrackingNumber = IGenerator.GenerateTrackingNumber();
            Status = ShippingStatus.Pending;
        }

        public static Shipment Create()
        {
            return new Shipment();
        }
    }
}
