using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Tag.Get
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQueryRequest, GetTagByIdQueryResponse>
    {
        private readonly ITagService _tagService;

        public GetTagByIdQueryHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<GetTagByIdQueryResponse> Handle(GetTagByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var tag = await _tagService.GetAsync(request.Id!, cancellationToken);

            return new GetTagByIdQueryResponse
            {
                IsSuccess = true,
                Data = tag
            };
        }
    }
}
