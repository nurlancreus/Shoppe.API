using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reply.Update
{
    public class UpdateReplyCommandRequest : IRequest<UpdateReplyCommandResponse>
    {
        public Guid? Id { get; set; }
        public string? Body { get; set; }
    }
}
