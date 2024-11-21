using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Contact.GetAllContacts
{
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQueryRequest, GetAllContactsQueryResponse>
    {
        private readonly IContactService _contactService;

        public GetAllContactsQueryHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<GetAllContactsQueryResponse> Handle(GetAllContactsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _contactService.GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return new GetAllContactsQueryResponse()
            {
                IsSuccess = true,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Data = result.Contacts,
            };
        }
    }
}
