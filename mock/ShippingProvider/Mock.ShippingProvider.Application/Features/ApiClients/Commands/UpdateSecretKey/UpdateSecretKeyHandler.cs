using MediatR;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Responses;
using Mock.ShippingProvider.Domain.Entities;
using Mock.ShippingProvider.Domain;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.UpdateSecretKey
{
    public class UpdateSecretKeyHandler : IRequestHandler<UpdateSecretKeyCommand, ResponseWithData<ApiClientDTO>>
    {
        private readonly IApiClientRepository _apiClientRepository;

        public UpdateSecretKeyHandler(IApiClientRepository apiClientRepository)
        {
            _apiClientRepository = apiClientRepository;
        }

        public async Task<ResponseWithData<ApiClientDTO>> Handle(UpdateSecretKeyCommand request, CancellationToken cancellationToken)
        {
            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(request.Id, true, cancellationToken);

            if (apiClient is null)
            {
                return new ResponseWithData<ApiClientDTO>
                {
                    IsSuccess = false,
                    Message = "Api client not found"
                };
            }

            apiClient.SecretKey = IGenerator.GenerateSecretKey();

            await _apiClientRepository.SaveChangesAsync(cancellationToken);

            return new ResponseWithData<ApiClientDTO>
            {
                IsSuccess = true,
                Message = "Secret key updated successfully",
                Data = new ApiClientDTO(apiClient)
            };
        }
    }
}
