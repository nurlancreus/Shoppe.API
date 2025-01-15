using MediatR;
using Mock.ShippingProvider.Application.Features.ApiClients.DTOs;
using Mock.ShippingProvider.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.Create
{
    public record CreateApiClientCommand : IRequest<ResponseWithData<ApiClientDTO>>
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string? State { get; set; }
    }
}
