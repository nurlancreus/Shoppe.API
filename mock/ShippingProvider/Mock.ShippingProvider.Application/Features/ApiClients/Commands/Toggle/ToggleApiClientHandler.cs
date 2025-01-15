using MediatR;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Responses;
using Mock.ShippingProvider.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.Toggle
{
    public class ToggleApiClientHandler : IRequestHandler<ToggleApiClientCommand, BaseResponse>
    {
        private readonly IApiClientRepository _apiClientRepository;

        public ToggleApiClientHandler(IApiClientRepository apiClientRepository)
        {
            _apiClientRepository = apiClientRepository;
        }

        public async Task<BaseResponse> Handle(ToggleApiClientCommand request, CancellationToken cancellationToken)
        {
            ApiClient? apiClient = await _apiClientRepository.GetByIdAsync(request.Id, true, cancellationToken);

            if (apiClient is null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Api client not found"
                };
            }

            if (!apiClient.IsActive) apiClient.IsActive = true;  
            else apiClient.IsActive = false;
            

            await _apiClientRepository.SaveChangesAsync(cancellationToken);

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Api client updated successfully"
            };
        }
    }
}
