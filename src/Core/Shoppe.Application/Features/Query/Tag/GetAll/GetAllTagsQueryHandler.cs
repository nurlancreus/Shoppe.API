using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Tag.GetAll
{
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQueryRequest, GetAllTagsQueryResponse>
    {
        private readonly ITagService _tagService;

        public GetAllTagsQueryHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<GetAllTagsQueryResponse> Handle(GetAllTagsQueryRequest request, CancellationToken cancellationToken)
        {
            TagType? tagType = Enum.TryParse(request.Type, out TagType parsedType) ? parsedType : null;

            var result = await _tagService.GetAllAsync(request.Page, request.PageSize, tagType, cancellationToken);

            return new GetAllTagsQueryResponse
            {
                IsSuccess = true,
                PageSize = result.PageSize,
                Page = result.Page,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Tags,
            };
        }
    }
}
