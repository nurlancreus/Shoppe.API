using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reply.Delete
{
    public class DeleteReplyCommandRequest : IRequest<DeleteReplyCommandResponse>
    {
        public Guid? Id { get; set; }
    }
}
