﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Slider.Get
{
    public class GetSliderQueryRequest : IRequest<GetSliderQueryResponse>
    {
        public string? SliderId { get; set; }
    }
}