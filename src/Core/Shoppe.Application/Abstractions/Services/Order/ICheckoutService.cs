﻿using Shoppe.Application.DTOs.Checkout;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        Task<GetCheckoutResponseDTO> CheckoutAsync(CreateCheckoutDTO createCheckoutDTO, CancellationToken cancellationToken = default);

    }
}
