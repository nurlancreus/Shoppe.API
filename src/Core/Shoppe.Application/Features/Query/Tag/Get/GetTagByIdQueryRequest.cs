using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Tag.Get
{
    public class GetTagByIdQueryRequest : IRequest<GetTagByIdQueryResponse>
    {
        public Guid? Id { get; set; }
    }
}
