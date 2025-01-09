﻿using Shoppe.Application.DTOs.Location;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Location.GetCountries
{
    public class GetCountriesQueryResponse : AppResponseWithData<List<GetCountryDTO>>
    {
    }
}