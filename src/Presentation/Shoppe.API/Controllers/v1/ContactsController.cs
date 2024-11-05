using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Contact.CreateContact;
using Shoppe.Application.Features.Command.Contact.DeleteContact;
using Shoppe.Application.Features.Command.Contact.UpdateContact;
using Shoppe.Application.Features.Query.Contact.GetAllContacts;
using Shoppe.Application.Features.Query.Contact.GetContactById;

namespace Shoppe.API.Controllers.v1
{
    //[ApiVersion("1.0")]
    public class ContactsController : ApplicationControllerBase
    {
        private readonly ISender _mediator;

        public ContactsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllContactsQueryRequest getAllContactsQueryRequest)
        {
            var response = await _mediator.Send(getAllContactsQueryRequest);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var getContactByIdRequest = new GetContactByIdQueryRequest { Id = id };

            var response = await _mediator.Send(getContactByIdRequest);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactCommandRequest createContactCommandRequest)
        {

            var response = await _mediator.Send(createContactCommandRequest);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommandRequest updateContactCommandRequest)
        {
            updateContactCommandRequest.Id = id;

            var response = await _mediator.Send(updateContactCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteContactCommandRequest = new DeleteContactCommandRequest { Id = id };

            var response = await _mediator.Send(deleteContactCommandRequest);

            return Ok(response);
        }
    }
}
