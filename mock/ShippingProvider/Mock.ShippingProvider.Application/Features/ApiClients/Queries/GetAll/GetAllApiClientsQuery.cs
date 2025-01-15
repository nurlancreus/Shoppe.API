using MediatR;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Queries.GetAll
{
    public record GetAllApiClientsQuery : IRequest<ResponseWithData<IEnumerable<ApiClientDTO>>>
    {
    }

}
