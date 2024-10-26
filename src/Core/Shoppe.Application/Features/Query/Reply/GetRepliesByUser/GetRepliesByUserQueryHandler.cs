using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.GetRepliesByUser
{
    public class GetRepliesByUserQueryHandler : IRequestHandler<GetRepliesByUserQueryRequest, GetRepliesByUserQueryResponse>
    {
        private readonly IReplyService _replyService;

        public GetRepliesByUserQueryHandler(IReplyService replyService)
        {
            _replyService = replyService;
        }

        public async Task<GetRepliesByUserQueryResponse> Handle(GetRepliesByUserQueryRequest request, CancellationToken cancellationToken)
        {
            var replies = await _replyService.GetRepliesByUserAsync(request.UserId!, cancellationToken);

            return new GetRepliesByUserQueryResponse
            {
                IsSuccess = true,
                Data = replies,
            };
        }
    }
}
