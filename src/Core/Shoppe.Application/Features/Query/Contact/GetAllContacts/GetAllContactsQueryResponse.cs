using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Contact.GetAllContacts
{
    public class GetAllContactsQueryResponse : AppPaginatedResponse<List<GetContactDTO>>
    {
    }
}
