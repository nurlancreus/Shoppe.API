using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.OrderRepos;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using Shoppe.Domain.Exceptions.Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly CalculatorService _calculatorService;
        private readonly IPayPalService _payPalService;
        private readonly IJwtSession _jwtSession;

        public PaymentService(IOrderReadRepository orderReadRepository, CalculatorService calculatorService, IPayPalService payPalService, IJwtSession jwtSession)
        {
            _orderReadRepository = orderReadRepository;
            _calculatorService = calculatorService;
            _payPalService = payPalService;
            _jwtSession = jwtSession;
        }

        public async Task<string> CreatePaymentAsync(Guid orderId, PaymentMethod paymentMethod, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var order = await _orderReadRepository.Table
                                .Include(o => o.Basket)
                                    .ThenInclude(b => b.Items)
                                        .ThenInclude(bi => bi.Product)
                                            .ThenInclude(p => p.Discount)
                                .Include(o => o.Basket)
                                    .ThenInclude(b => b.Items)
                                        .ThenInclude(bi => bi.Product)
                                            .ThenInclude(p => p.Categories)
                                                .ThenInclude(c => c.Discount)
                                .Include(o => o.Basket)
                                    .ThenInclude(b => b.User)
                                .FirstOrDefaultAsync(o => o.Id == orderId && o.Basket.UserId == userId, cancellationToken);

            if (order == null)
                throw new EntityNotFoundException(nameof(order));


            var amount = _calculatorService.CalculateCouponAppliedPrice(order);

            return await (paymentMethod switch
            {
                PaymentMethod.PayPal => _payPalService.CreatePaymentAsync(amount, "USD", cancellationToken),
                PaymentMethod.DebitCard => throw new NotImplementedException(),
                PaymentMethod.CashOnDelivery => throw new NotImplementedException(),
                _ => throw new PaymentFailedException("Invalid payment method")
            });

        }
    }
}
