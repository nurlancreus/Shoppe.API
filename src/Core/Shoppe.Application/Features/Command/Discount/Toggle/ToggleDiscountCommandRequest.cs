using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.Toggle
{
    public class ToggleDiscountCommandRequest : IRequest<ToggleDiscountCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
