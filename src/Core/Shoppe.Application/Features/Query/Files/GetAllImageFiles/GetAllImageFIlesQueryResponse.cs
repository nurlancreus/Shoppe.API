using Shoppe.Application.DTOs.Files;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Files.GetAllImageFiles
{
    public class GetAllImageFIlesQueryResponse : AppResponseWithPaginatedData<List<GetFileDTO>>
    {
    }
}
