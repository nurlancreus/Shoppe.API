using MediatR;
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

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.Create
{
    public class CreateApiClientHandler : IRequestHandler<CreateApiClientCommand, ResponseWithData<ApiClientDTO>>
    {
        private readonly IApiClientRepository _apiClientRepository;
        private readonly IGeoInfoService _geoInfoService;

        public CreateApiClientHandler(IApiClientRepository apiClientRepository, IGeoInfoService geoInfoService)
        {
            _apiClientRepository = apiClientRepository;
            _geoInfoService = geoInfoService;
        }

        public async Task<ResponseWithData<ApiClientDTO>> Handle(CreateApiClientCommand request, CancellationToken cancellationToken)
        {
            var apiClient = ApiClient.Create(request.CompanyName);

            var response = await _geoInfoService.GetLocationGeoInfoByNameAsync(request.Country, request.City);

            var lat = response.Latitude;
            var lon = response.Longitude;

            var address = Address.Create(request.Country, request.City, request.State, request.PostalCode, request.Street, lat, lon);

            apiClient.Address = address;

            await _apiClientRepository.AddAsync(apiClient, cancellationToken);

            await _apiClientRepository.SaveChangesAsync(cancellationToken);

            var dto = new ApiClientDTO(apiClient);

            return new ResponseWithData<ApiClientDTO>
            {
                IsSuccess = true,
                Message = "Api client created successfully",
                Data = dto
            };
        }
    }
}
