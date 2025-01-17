﻿using Mock.ShippingProvider.Domain.Entities.Base;

namespace Mock.ShippingProvider.Domain.Entities
{
    public class ApiClient : BaseEntity
    {
        public string CompanyName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;  // Public API Key
        public string SecretKey { get; set; } = string.Empty;  // Secret Key for HMAC
        public bool IsActive { get; set; } = true;

        public Address Address { get; set; } = null!;  // Navigation Property to Address

        public ICollection<Shipment> Shipments { get; set; } = []; // Navigation Property to Shipments

        private ApiClient(string companyName)
        {
            CompanyName = companyName;
            ApiKey = IGenerator.GenerateApiKey();
            SecretKey = IGenerator.GenerateSecretKey();
            IsActive = true;
        }

        public static ApiClient Create(string companyName)
        {
            return new ApiClient(companyName);
        }
    }
}
