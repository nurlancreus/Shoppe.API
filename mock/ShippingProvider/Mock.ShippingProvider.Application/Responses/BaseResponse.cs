﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
