using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Discount.DeleteDiscount
{
    public class DeleteDiscountCommandRequest : IRequest<DeleteDiscountCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
