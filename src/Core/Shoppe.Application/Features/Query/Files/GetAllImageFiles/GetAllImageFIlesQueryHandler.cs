using MediatR;
using Shoppe.Application.Abstractions.Services.Files.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Files.GetAllImageFiles
{
    public class GetAllImageFIlesQueryHandler : IRequestHandler<GetAllImageFIlesQueryRequest, GetAllImageFIlesQueryResponse>
    {
        private readonly IApplicationImageFileService _applicationImageFIleService;

        public GetAllImageFIlesQueryHandler(IApplicationImageFileService applicationImageFIleService)
        {
            _applicationImageFIleService = applicationImageFIleService;
        }

        public async Task<GetAllImageFIlesQueryResponse> Handle(GetAllImageFIlesQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _applicationImageFIleService.GetAllImagesDTO(request.Page, request.PageSize, request.Type, cancellationToken);

            return new GetAllImageFIlesQueryResponse
            {
                IsSuccess = true,
                PageSize = result.PageSize,
                Page = result.Page,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Images
            };
        }
    }
}
