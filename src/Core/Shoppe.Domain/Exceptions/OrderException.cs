using Shoppe.Domain.Exceptions.Base;
using System;
using System.Net;

namespace Shoppe.Domain.Exceptions
{
    public class OrderException : BaseException
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string? CustomerId { get; set; }
        public string? Reason { get; set; }

        public override string Title { get; set; } = "Order Failed";
        public override string Description { get; set; } = "An error occurred while processing the order.";

        public OrderException() { }

        public OrderException(string message) : base(message, HttpStatusCode.BadRequest) { }

        public OrderException(string message, string orderNumber) : base(message)
        {
            OrderNumber = orderNumber;
        }

        public OrderException(string message, string orderNumber, string? customerId) : base(message, HttpStatusCode.BadRequest)
        {
            OrderNumber = orderNumber;
            CustomerId = customerId;
        }

        public OrderException(string message, string orderNumber, string? customerId, string? reason) : base(message, HttpStatusCode.BadRequest)
        {
            OrderNumber = orderNumber;
            CustomerId = customerId;
            Reason = reason;
        }

        public OrderException(string message, HttpStatusCode statusCode) : base(message, statusCode) { }

        public OrderException(string message, string orderNumber, HttpStatusCode statusCode) : base(message, statusCode)
        {
            OrderNumber = orderNumber;
        }

        public OrderException(string message, string orderNumber, string? customerId, string? reason, HttpStatusCode statusCode) : base(message, statusCode)
        {
            OrderNumber = orderNumber;
            CustomerId = customerId;
            Reason = reason;
        }

        public OrderException(string message, Exception innerException) : base(message, innerException) { }

        public OrderException(string message, string orderNumber, Exception innerException) : base(message, HttpStatusCode.BadRequest, innerException)
        {
            OrderNumber = orderNumber;
        }

        public OrderException(string message, string orderNumber, string? customerId, Exception innerException) : base(message, HttpStatusCode.BadRequest, innerException)
        {
            OrderNumber = orderNumber;
            CustomerId = customerId;
        }
    }
}
