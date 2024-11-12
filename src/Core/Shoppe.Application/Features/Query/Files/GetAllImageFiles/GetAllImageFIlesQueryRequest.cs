using MediatR;
using Shoppe.Application.RequestParams;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Files.GetAllImageFiles
{
    public class GetAllImageFIlesQueryRequest : PaginationRequestParams, IRequest<GetAllImageFIlesQueryResponse>
    {
        public ImageFileType? Type { get; set; }
    }
}
