using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.GetRepliesByEntity
{
    public class GetRepliesByEntityQueryHandler : IRequestHandler<GetRepliesByEntityQueryRequest, GetRepliesByEntityQueryResponse>
    {
        private readonly IReplyService _replyService;

        public GetRepliesByEntityQueryHandler(IReplyService replyService)
        {
            _replyService = replyService;
        }

        public async Task<GetRepliesByEntityQueryResponse> Handle(GetRepliesByEntityQueryRequest request, CancellationToken cancellationToken)
        {
            var replies = await _replyService.GetRepliesByEntityAsync(request.EntityId!, request.ReplyType, cancellationToken);

            return new GetRepliesByEntityQueryResponse
            {
                IsSuccess = true,
                Data = replies,
            };
        }
    }
}
