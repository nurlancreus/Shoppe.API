using Shoppe.Application.DTOs.User;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.User.GetAll
{
    public class GetAllUsersQueryResponse : AppResponseWithPaginatedData<List<GetUserDTO>>
    {
    }
}
