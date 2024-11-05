using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Contact.DeleteContact
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommandRequest, DeleteContactCommandResponse>
    {
        private readonly IContactService _contactService;

        public DeleteContactCommandHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<DeleteContactCommandResponse> Handle(DeleteContactCommandRequest request, CancellationToken cancellationToken)
        {
            await _contactService.DeleteContactAsync((Guid)request.Id!, cancellationToken);

            return new DeleteContactCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.DeletedSuccessMessage("Contact")
            };
        }
    }
}
