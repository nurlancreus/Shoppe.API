﻿using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Features.Shipments.DTOs;
using Mock.ShippingProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.Addresses.DTOs
{
    public record AddressDTO
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApiClientDTO? Client { get; set; }
        public ICollection<ShipmentDTO> ShipmentsOrigin { get; set; } = [];
        public ICollection<ShipmentDTO> ShipmentsDestination { get; set; } = [];
        public AddressDTO() { }
        public AddressDTO(Address address)
        {
            Id = address.Id;
            Street = address.Street;
            City = address.City;
            State = address.State;
            PostalCode = address.PostalCode;
            Country = address.Country;
            Latitude = address.Latitude;
            Longitude = address.Longitude;
            CreatedAt = address.CreatedAt;
            Client = address.Client is not null ? new ApiClientDTO(address.Client) : null;
            ShipmentsOrigin = address.ShipmentsOrigin.Select(s => new ShipmentDTO(s)).ToList();
            ShipmentsDestination = address.ShipmentsDestination.Select(s => new ShipmentDTO(s)).ToList();
        }
    }
}
