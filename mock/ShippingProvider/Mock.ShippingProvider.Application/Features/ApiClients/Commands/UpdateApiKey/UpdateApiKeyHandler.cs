using MediatR;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Responses;
using Mock.ShippingProvider.Domain.Entities;
using Mock.ShippingProvider.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.UpdateApiKey
{
    public class UpdateApiKeyHandler : IRequestHandler<UpdateApiKeyCommand, ResponseWithData<ApiClientDTO>>
    {
        private readonly IApiClientRepository _apiClientRepository;

        public UpdateApiKeyHandler(IApiClientRepository apiClientRepository)
        {
            _apiClientRepository = apiClientRepository;
        }

        public async Task<ResponseWithData<ApiClientDTO>> Handle(UpdateApiKeyCommand request, CancellationToken cancellationToken)
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

            apiClient.ApiKey = IGenerator.GenerateApiKey();

            await _apiClientRepository.SaveChangesAsync(cancellationToken);

            return new ResponseWithData<ApiClientDTO>
            {
                IsSuccess = true,
                Message = "Api key updated successfully",
                Data = new ApiClientDTO(apiClient)
            };
        }
    }
}
