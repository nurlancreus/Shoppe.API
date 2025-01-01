using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Exceptions
{
    using global::Shoppe.Domain.Exceptions.Base;
    using System;
    using System.Net;

    namespace Shoppe.Domain.Exceptions
    {
        public class PaymentFailedException : BaseException
        {
            public string PaymentMethod { get; set; } = string.Empty;
            public string? PaymentTransactionId { get; set; }

            public override string Title { get; set; } = "Payment Failed";
            public override string Description { get; set; } = "An error occurred while processing the payment.";

            public PaymentFailedException()
            {
            }

            public PaymentFailedException(string message) : base(message, HttpStatusCode.PaymentRequired)
            {
            }

            public PaymentFailedException(string message, string paymentMethod) : base(message)
            {
                PaymentMethod = paymentMethod;
            }

            public PaymentFailedException(string message, string paymentMethod, string? paymentTransactionId) : base(message, HttpStatusCode.PaymentRequired)
            {
                PaymentMethod = paymentMethod;
                PaymentTransactionId = paymentTransactionId;
            }

            public PaymentFailedException(string message, HttpStatusCode statusCode) : base(message, statusCode)
            {
            }

            public PaymentFailedException(string message, string paymentMethod, string? paymentTransactionId, HttpStatusCode statusCode) : base(message, statusCode)
            {
                PaymentMethod = paymentMethod;
                PaymentTransactionId = paymentTransactionId;
            }

            public PaymentFailedException(string message, string paymentMethod, HttpStatusCode statusCode) : base(message, statusCode)
            {
                PaymentMethod = paymentMethod;
            }

            public PaymentFailedException(string message, Exception innerException) : base(message, innerException)
            {
            }

            public PaymentFailedException(string message, string paymentMethod, Exception innerException) : base(message, HttpStatusCode.PaymentRequired, innerException)
            {
                PaymentMethod = paymentMethod;
            }
        }
    }

}
