﻿using MediatR;
using Mock.ShippingProvider.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Features.ApiClients.Commands.Toggle
{
    public record ToggleApiClientCommand : IRequest<BaseResponse>
    {
        public Guid Id { get; set; }
    }
}