﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppe.Application.Features.Command.Discount.AssignDiscount;
using Shoppe.Application.Features.Command.Discount.CreateDiscount;
using Shoppe.Application.Features.Command.Discount.DeleteDiscount;
using Shoppe.Application.Features.Command.Discount.Toggle;
using Shoppe.Application.Features.Command.Discount.UpdateDiscount;
using Shoppe.Application.Features.Query.Discount.Get;
using Shoppe.Application.Features.Query.Discount.GetAll;

namespace Shoppe.API.Controllers.v1
{
    [Authorize(ApiConstants.AuthPolicies.AdminsPolicy)]
    public class DiscountsController : ApplicationVersionController
    {
        private readonly ISender _sender;

        public DiscountsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateDiscountCommandRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDiscountsQueryRequest request)
        {
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetDiscountByIdQueryRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateDiscountCommandRequest request)
        {
            request.Id = id;
            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteDiscountCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }

        [HttpPatch("{id}/entity/{entityId}")]
        public async Task<IActionResult> AssignDiscount(Guid id, Guid entityId, [FromQuery] string entityType)
        {
            var request = new AssignDiscountCommandRequest
            {
                DiscountId = id,
                EntityId = entityId,
                EntityType = entityType
            };

            var response = await _sender.Send(request);

            return Ok(response);
        }


        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            var request = new ToggleDiscountCommandRequest { Id = id };

            var response = await _sender.Send(request);

            return Ok(response);
        }
    }
}
