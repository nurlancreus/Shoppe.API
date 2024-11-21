using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.DTOs.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Contact.AnswerContact
{
    public class AnswerContactCommandHandler : IRequestHandler<AnswerContactCommandRequest, AnswerContactCommandResponse>
    {
        private readonly IContactService _contactService;

        public AnswerContactCommandHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<AnswerContactCommandResponse> Handle(AnswerContactCommandRequest request, CancellationToken cancellationToken)
        {
            await _contactService.AnswerContactMessageAsync(new AnswerContactMessageDTO
            {
                ContactId = (Guid)request.ContactId!,
                Message = request.Message,
            }, cancellationToken);

            return new AnswerContactCommandResponse
            {
                IsSuccess = true
            };
        }
    }
}
