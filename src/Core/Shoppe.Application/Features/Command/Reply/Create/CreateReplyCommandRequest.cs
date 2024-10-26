using MediatR;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Reply.Create
{
    public class CreateReplyCommandRequest : IRequest<CreateReplyCommandResponse>
    {
        public string Body { get; set; } = string.Empty;
        public string? EntityId { get; set; } = null!;
        public ReplyType Type { get; set; } = ReplyType.Blog;
    }
}
