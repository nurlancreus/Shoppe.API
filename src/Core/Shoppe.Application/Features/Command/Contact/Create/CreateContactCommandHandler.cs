using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Contact.CreateContact
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommandRequest, CreateContactCommandResponse>
    {
        private readonly IContactService _contactService;

        public CreateContactCommandHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<CreateContactCommandResponse> Handle(CreateContactCommandRequest request, CancellationToken cancellationToken)
        {
            await _contactService.CreateAsync(request.ToCreateContactDTO(), cancellationToken);

            return new CreateContactCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.AddedSuccessMessage("Contact")
            };
        }
    }
}
