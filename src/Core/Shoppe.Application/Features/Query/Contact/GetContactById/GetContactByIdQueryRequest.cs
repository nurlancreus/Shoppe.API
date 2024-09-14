using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Contact.GetContactById
{
    public class GetContactByIdQueryRequest : IRequest<GetContactByIdQueryResponse>
    {
        public string? Id {  get; set; }
    }
}
