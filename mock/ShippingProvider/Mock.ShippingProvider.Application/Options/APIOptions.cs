﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Options
{
    public class APIOptions
    {
        public const string GeoCodeAPI = "GeoCodeAPI";
        public string BaseUrl { get; set; } = string.Empty;
        public float? Version { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiSecret { get; set; }
    }
}
