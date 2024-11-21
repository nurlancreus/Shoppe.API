using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using Shoppe.Application.Extensions.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Contact.UpdateContact
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommandRequest, UpdateContactCommandResponse>
    {
        private readonly IContactService _contactService;

        public UpdateContactCommandHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<UpdateContactCommandResponse> Handle(UpdateContactCommandRequest request, CancellationToken cancellationToken)
        {
            await _contactService.UpdateAsync(request.ToUpdateContactDTO(), cancellationToken);

            return new UpdateContactCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.UpdatedSuccessMessage("Contact")
            };
        }
    }
}
