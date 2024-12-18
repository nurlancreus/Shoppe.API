using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Location.GetCountries
{
    public class GetCountriesQueryRequest : IRequest<GetCountriesQueryResponse>
    {
    }
}
