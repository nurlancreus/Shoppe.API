using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Features.Query.Reaction.GetBlogReactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reaction.GetReplyReactions
{
    public class GetReplyReactionsQueryHandler : IRequestHandler<GetReplyReactionsQueryRequest, GetReplyReactionsQueryResponse>
    {
        private readonly IReactionService _reactionService;

        public GetReplyReactionsQueryHandler(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        public async Task<GetReplyReactionsQueryResponse> Handle(GetReplyReactionsQueryRequest request, CancellationToken cancellationToken)
        {
            var replyReactions = await _reactionService.GetReplyReactionsAsync(request.ReplyId, cancellationToken);

            return new GetReplyReactionsQueryResponse
            {
                Data = replyReactions,
            };
        }
    }
}
