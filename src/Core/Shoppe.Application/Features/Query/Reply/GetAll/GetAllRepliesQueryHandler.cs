using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.GetAll
{
    public class GetAllRepliesQueryHandler : IRequestHandler<GetAllRepliesQueryRequest, GetAllRepliesQueryResponse>
    {
        private readonly IReplyService _replyService;

        public GetAllRepliesQueryHandler(IReplyService replyService)
        {
            _replyService = replyService;
        }

        public async Task<GetAllRepliesQueryResponse> Handle(GetAllRepliesQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _replyService.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllRepliesQueryResponse
            {
                IsSuccess = true,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Replies,
            };
        }
    }
}
