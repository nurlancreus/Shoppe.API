using MediatR;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Interfaces.Repositories;
using Mock.ShippingProvider.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Queries.GetAll
{
    public class GetAllApiClientsHandler : IRequestHandler<GetAllApiClientsQuery, ResponseWithData<IEnumerable<ApiClientDTO>>>
    {
        private readonly IApiClientRepository _apiClientRepository;

        public GetAllApiClientsHandler(IApiClientRepository apiClientRepository)
        {
            _apiClientRepository = apiClientRepository;
        }

        public async Task<ResponseWithData<IEnumerable<ApiClientDTO>>> Handle(GetAllApiClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await _apiClientRepository.GetAllAsync(false, cancellationToken);

            var dtos = clients.Select(client => new ApiClientDTO(client));

            return new ResponseWithData<IEnumerable<ApiClientDTO>>
            {
                IsSuccess = true,
                Message = "Api clients retrieved successfully",
                Data = dtos
            };
        }
    }
}
