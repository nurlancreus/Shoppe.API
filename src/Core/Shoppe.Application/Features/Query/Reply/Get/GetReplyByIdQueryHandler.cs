using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Reply.Get
{
    public class GetReplyByIdQueryHandler : IRequestHandler<GetReplyByIdQueryRequest, GetReplyByIdQueryResponse>
    {
        private readonly IReplyService _replyService;

        public GetReplyByIdQueryHandler(IReplyService replyService)
        {
            _replyService = replyService;
        }

        public async Task<GetReplyByIdQueryResponse> Handle(GetReplyByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var reply = await _replyService.GetAsync((Guid)request.Id!, cancellationToken);

            return new GetReplyByIdQueryResponse
            {
                IsSuccess = true,
                Data = reply
            };
        }
    }
}
