using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Contact.GetContactById
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQueryRequest, GetContactByIdQueryResponse>
    {
        private readonly IContactService _contactService;

        public GetContactByIdQueryHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<GetContactByIdQueryResponse> Handle(GetContactByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var contact = await _contactService.GetContactAsync((Guid)request.Id!, cancellationToken);

            return new GetContactByIdQueryResponse()
            {
                IsSuccess = true,
                Data = contact
            };
        }
    }
}
