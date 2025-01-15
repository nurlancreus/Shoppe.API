using MediatR;
using Microsoft.EntityFrameworkCore;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Interfaces.Services;
using Mock.ShippingProvider.Application.Responses;
using Mock.ShippingProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.Update
{
    public class UpdateApiClientHandler : IRequestHandler<UpdateApiClientCommand, ResponseWithData<ApiClientDTO>>
    {
        private readonly IApiClientRepository _apiClientRepository;
        private readonly IGeoInfoService _geoInfoService;

        public UpdateApiClientHandler(IApiClientRepository apiClientRepository, IGeoInfoService geoInfoService)
        {
            _apiClientRepository = apiClientRepository;
            _geoInfoService = geoInfoService;
        }

        public async Task<ResponseWithData<ApiClientDTO>> Handle(UpdateApiClientCommand request, CancellationToken cancellationToken)
        {
            ApiClient? apiClient = await _apiClientRepository.Table
                                           .Include(client => client.Address)
                                           .FirstOrDefaultAsync(client => client.Id == request.Id, cancellationToken);

            if (apiClient is null)
            {
                return new ResponseWithData<ApiClientDTO>
                {
                    IsSuccess = false,
                    Message = "Api client not found",
                };
            }

            if (request.CompanyName is string companyName && companyName != apiClient.CompanyName)
            {
                apiClient.CompanyName = companyName;
            }

            if (request.Country is string country && country != apiClient.Address.Country)
            {
                if (request.City is not string city) return new ResponseWithData<ApiClientDTO>
                {
                    IsSuccess = false,
                    Message = "City is required",
                };

                var response = await _geoInfoService.GetLocationGeoInfoByNameAsync(country, city);
                var lat = response.Latitude;
                var lon = response.Longitude;

                apiClient.Address.Country = country;
                apiClient.Address.City = city;

                apiClient.Address.Latitude = lat;
                apiClient.Address.Longitude = lon;
            }
            else if (request.City is string city && city != apiClient.Address.City)
            {
                var response = await _geoInfoService.GetLocationGeoInfoByNameAsync(apiClient.Address.Country, city);
                var lat = response.Latitude;
                var lon = response.Longitude;
                apiClient.Address.City = city;
                apiClient.Address.Latitude = lat;
                apiClient.Address.Longitude = lon;
            }

            if (request.State is string state && state != apiClient.Address.State)
            {
                apiClient.Address.State = state;
            }

            if (request.PostalCode is string postalCode && postalCode != apiClient.Address.PostalCode)
            {
                apiClient.Address.PostalCode = postalCode;
            }

            if (request.Street is string street && street != apiClient.Address.Street)
            {
                apiClient.Address.Street = street;
            }

            await _apiClientRepository.SaveChangesAsync(cancellationToken);

            return new ResponseWithData<ApiClientDTO>
            {
                IsSuccess = true,
                Message = "Api client updated successfully",
                Data = new ApiClientDTO(apiClient)
            };
        }
    }
}
