using MediatR;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Queries.Get
{
    public record GetApiClientByIdQuery : IRequest<ResponseWithData<ApiClientDTO>>
    {
        public Guid Id { get; set; }
    }
}
