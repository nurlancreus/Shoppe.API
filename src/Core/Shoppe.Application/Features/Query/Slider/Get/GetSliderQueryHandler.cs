using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Slider.Get
{
    public class GetSliderQueryHandler : IRequestHandler<GetSliderQueryRequest, GetSliderQueryResponse>
    {
        public Task<GetSliderQueryResponse> Handle(GetSliderQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
