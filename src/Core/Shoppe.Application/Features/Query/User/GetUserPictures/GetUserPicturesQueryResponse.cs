using Shoppe.Application.DTOs.Files;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.User.GetUserPictures
{
    public class GetUserPicturesQueryResponse : AppResponseWithData<List<GetImageFileDTO>>
    {
    }
}
