using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Location.GetCititesByCountry
{
    public class GetCitiesByCountryQueryRequest : IRequest<GetCitiesByCountryQueryResponse>
    {
        public Guid? CountryId { get; set; }
    }
}
