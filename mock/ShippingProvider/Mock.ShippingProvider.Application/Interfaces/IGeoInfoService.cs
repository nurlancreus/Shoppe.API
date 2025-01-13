using Mock.ShippingProvider.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Interfaces
{
    public interface IGeoInfoService
    {
        Task<CountryGeoInfo> GetCountryGeoInfoByNameAsync(string countryName);

    }
}
