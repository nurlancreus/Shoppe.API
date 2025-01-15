using MediatR;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Responses;
using Mock.ShippingProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Queries.Get
{
    public class GetApiClientByIdHandler : IRequestHandler<GetApiClientByIdQuery, ResponseWithData<ApiClientDTO>>
    {
        private readonly IApiClientRepository _apiClientRepository;

        public GetApiClientByIdHandler(IApiClientRepository apiClientRepository)
        {
            _apiClientRepository = apiClientRepository;
        }

        public async Task<ResponseWithData<ApiClientDTO>> Handle(GetApiClientByIdQuery request, CancellationToken cancellationToken)
        {
            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(request.Id, false, cancellationToken);

            if (apiClient is null)
            {
                return new ResponseWithData<ApiClientDTO>
                {
                    IsSuccess = false,
                    Message = "Api client not found"
                };
            }

            return new ResponseWithData<ApiClientDTO>
            {
                IsSuccess = true,
                Message = "Api client retrieved successfully",
                Data = new ApiClientDTO(apiClient)
            };
        }
    }
}
