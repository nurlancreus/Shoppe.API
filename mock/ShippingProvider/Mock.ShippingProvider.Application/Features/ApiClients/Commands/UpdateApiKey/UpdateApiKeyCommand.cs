using MediatR;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.UpdateApiKey
{
    public record UpdateApiKeyCommand : IRequest<ResponseWithData<ApiClientDTO>>
    {
        public Guid Id { get; set; }
    }
}
