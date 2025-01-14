//using Microsoft.EntityFrameworkCore;
//using Mock.ShippingProvider.Application.DTOs;
//using Mock.ShippingProvider.Application.Interfaces.Repositories;
//using Mock.ShippingProvider.Application.Interfaces.Services;
//using Mock.ShippingProvider.Domain.Entities;

//namespace Mock.ShippingProvider.Infrastructure.Persistence.Services
//{
//    public class ApiClientService(IApiClientRepository apiClientRepository, IGeoInfoService geoInfoService) : IApiClientService
//    {
//        private readonly IApiClientRepository _apiClientRepository = apiClientRepository;
//        private readonly IGeoInfoService _geoInfoService = geoInfoService;

//        public async Task<ApiClientDTO> CreateAsync(CreateApiClientRequestDTO request, CancellationToken cancellationToken = default)
//        {
//            var apiClient = ApiClient.Create(request.CompanyName);

//            apiClient.ApiKey = IKeyGenerator.GenerateApiKey();
//            apiClient.SecretKey = IKeyGenerator.GenerateSecretKey();

//            var response = await _geoInfoService.GetLocationGeoInfoByNameAsync(request.Country, request.City);

//            var lat = response.Latitude;
//            var lon = response.Longitude;

//            var address = Address.Create(request.Country, request.City, request.State, request.PostalCode, request.Street, lat, lon);

//            apiClient.Address = address;

//            await _apiClientRepository.AddAsync(apiClient, cancellationToken);

//            await _apiClientRepository.SaveChangesAsync(cancellationToken);

//            return new ApiClientDTO(apiClient);
//        }

//        public async Task<ApiClientDTO> UpdateAsync(UpdateApiClientRequestDTO request, CancellationToken cancellationToken = default)
//        {
//            ApiClient? apiClient = await _apiClientRepository.Table
//                                            .Include(client => client.Address)
//                                            .FirstOrDefaultAsync(client => client.Id == request.Id, cancellationToken);

//            if (apiClient is null)
//            {
//                return null;
//            }

//            if (request.CompanyName is string companyName && companyName != apiClient.CompanyName)
//            {
//                apiClient.CompanyName = companyName;
//            }

//            if (request.Country is string country && country != apiClient.Address.Country)
//            {
//                if (request.City is not string city) return null;

//                var response = await _geoInfoService.GetLocationGeoInfoByNameAsync(country, city);
//                var lat = response.Latitude;
//                var lon = response.Longitude;

//                apiClient.Address.Country = country;
//                apiClient.Address.City = city;

//                apiClient.Address.Latitude = lat;
//                apiClient.Address.Longitude = lon;
//            }
//            else if (request.City is string city && city != apiClient.Address.City)
//            {
//                var response = await _geoInfoService.GetLocationGeoInfoByNameAsync(apiClient.Address.Country, city);
//                var lat = response.Latitude;
//                var lon = response.Longitude;
//                apiClient.Address.City = city;
//                apiClient.Address.Latitude = lat;
//                apiClient.Address.Longitude = lon;
//            }

//            if (request.State is string state && state != apiClient.Address.State)
//            {
//                apiClient.Address.State = state;
//            }

//            if (request.PostalCode is string postalCode && postalCode != apiClient.Address.PostalCode)
//            {
//                apiClient.Address.PostalCode = postalCode;
//            }

//            if (request.Street is string street && street != apiClient.Address.Street)
//            {
//                apiClient.Address.Street = street;
//            }

//            await _apiClientRepository.SaveChangesAsync(cancellationToken);

//            return new ApiClientDTO(apiClient);
//        }

//        public async Task<bool> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(id, true, cancellationToken);

//            if (apiClient is null)
//            {
//                return false;
//            }

//            if (apiClient.IsActive)
//            {
//                apiClient.IsActive = false;
//                await _apiClientRepository.SaveChangesAsync(cancellationToken);
//            }

//            return true;
//        }

//        public async Task<bool> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(id, true, cancellationToken);

//            if (apiClient is null)
//            {
//                return false;
//            }

//            if (!apiClient.IsActive)
//            {
//                apiClient.IsActive = true;
//                await _apiClientRepository.SaveChangesAsync(cancellationToken);
//            }

//            return true;
//        }

//        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(id, true, cancellationToken);

//            if (apiClient is null)
//            {
//                return false;
//            }

//            _apiClientRepository.Delete(apiClient);

//            await _apiClientRepository.SaveChangesAsync(cancellationToken);

//            return true;
//        }

//        public async Task<IEnumerable<ApiClientDTO>> GetAllAsync(CancellationToken cancellationToken = default)
//        {
//            var clients = await _apiClientRepository.GetAllAsync(false, cancellationToken);

//            return clients.Select(client => new ApiClientDTO(client));
//        }

//        public async Task<ApiClientDTO?> GetIdAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(id, true, cancellationToken);

//            if (apiClient is null)
//            {
//                return null;
//            }

//            return new ApiClientDTO(apiClient);
//        }

//        public async Task<bool> UpdateApiKeyAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(id, true, cancellationToken);

//            if (apiClient is null)
//            {
//                return false;
//            }

//            apiClient.ApiKey = IKeyGenerator.GenerateApiKey();

//            await _apiClientRepository.SaveChangesAsync(cancellationToken);

//            return true;
//        }

//        public async Task<bool> UpdateSecretKeyAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(id, true, cancellationToken);

//            if (apiClient is null)
//            {
//                return false;
//            }

//            apiClient.SecretKey = IKeyGenerator.GenerateSecretKey();

//            await _apiClientRepository.SaveChangesAsync(cancellationToken);

//            return true;
//        }
//    }
//}
