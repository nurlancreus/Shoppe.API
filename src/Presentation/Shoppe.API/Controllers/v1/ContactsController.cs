using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Contact.AnswerContact;
using Shoppe.Application.Features.Command.Contact.CreateContact;
using Shoppe.Application.Features.Command.Contact.DeleteContact;
using Shoppe.Application.Features.Command.Contact.UpdateContact;
using Shoppe.Application.Features.Query.Contact.GetAllContacts;
using Shoppe.Application.Features.Query.Contact.GetContactById;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Enums;

namespace Shoppe.API.Controllers.v1
{
    public class ContactsController : ApplicationVersionController
    {
        private readonly ISender _sender;

        public ContactsController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(ApiConstants.AuthPolicies.AdminsPolicy)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllContactsQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [Authorize(ApiConstants.AuthPolicies.AdminsPolicy)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetContactByIdQueryRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [Authorize(ApiConstants.AuthPolicies.AdminsPolicy)]
        [HttpPost("{id}")]
        public async Task<IActionResult> Answer(Guid id, [FromBody] AnswerContactCommandRequest request)
        {
            request.ContactId = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommandRequest request)
        {
            request.Id = id;

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [Authorize(ApiConstants.AuthPolicies.AdminsPolicy)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteContactCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = Enum.GetNames<ContactSubject>();
            var response = subjects.Select(s => new { Value = s, Label = StringHelpers.SplitAndJoinString(s, '_', ' ') });

            return Ok(await Task.FromResult(response));
        }
    }
}
