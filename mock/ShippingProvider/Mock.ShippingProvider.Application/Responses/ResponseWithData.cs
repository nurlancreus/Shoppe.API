﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Responses
{
    public class ResponseWithData<T> : BaseResponse where T : class?
    {
        public T? Data { get; set; }
    }
}