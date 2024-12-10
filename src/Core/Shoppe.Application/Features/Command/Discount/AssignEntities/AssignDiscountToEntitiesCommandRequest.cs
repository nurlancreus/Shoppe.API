using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.AssignEntities
{
    public class AssignDiscountToEntitiesCommandRequest : IRequest<AssignDiscountToEntitiesCommandResponse>
    {
        public Guid? Id { get; set; }
        public List<Guid>? ProductIds { get; set; }
        public List<Guid>? CategoryIds { get; set; }
    }
}
