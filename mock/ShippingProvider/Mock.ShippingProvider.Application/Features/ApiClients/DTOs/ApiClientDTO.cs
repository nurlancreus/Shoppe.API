﻿using Mock.ShippingProvider.Application.Features.Addresses.DTOs;
using Mock.ShippingProvider.Application.Features.Shipments.DTOs;
using Mock.ShippingProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.DTOs
{
    public record ApiClientDTO
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string? State { get; set; }
        public DateTime CreatedAt { get; set; }
        public AddressDTO? Address { get; set; }  // Navigation Property to Address
        public ICollection<ShipmentDTO> Shipments { get; set; } = []; // Navigation Property to Shipments

        public ApiClientDTO() { }

        public ApiClientDTO(ApiClient apiClient)
        {
            Id = apiClient.Id;
            CompanyName = apiClient.CompanyName;
            ApiKey = apiClient.ApiKey;
            SecretKey = apiClient.SecretKey;
            IsActive = apiClient.IsActive;
            Country = apiClient.Address.Country;
            City = apiClient.Address.City;
            Street = apiClient.Address.Street;
            PostalCode = apiClient.Address.PostalCode;
            State = apiClient.Address.State;
            CreatedAt = apiClient.CreatedAt;
            Address = new AddressDTO(apiClient.Address);
            Shipments = apiClient.Shipments.Select(s => new ShipmentDTO(s)).ToList();
        }
    }
}
