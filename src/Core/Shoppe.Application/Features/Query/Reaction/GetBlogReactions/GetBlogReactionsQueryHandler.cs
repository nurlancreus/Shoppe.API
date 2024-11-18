using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reaction.GetBlogReactions
{
    public class GetBlogReactionsQueryHandler : IRequestHandler<GetBlogReactionsQueryRequest, GetBlogReactionsQueryResponse>
    {
        private readonly IReactionService _reactionService;

        public GetBlogReactionsQueryHandler(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        public async Task<GetBlogReactionsQueryResponse> Handle(GetBlogReactionsQueryRequest request, CancellationToken cancellationToken)
        {
            var blogReactions = await _reactionService.GetBlogReactionsAsync(request.BlogId, cancellationToken);

            return new GetBlogReactionsQueryResponse
            {
                Data = blogReactions,
            };
        }
    }
}
