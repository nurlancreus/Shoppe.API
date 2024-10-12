using Shoppe.Domain.Exceptions.Base;
using System;
using System.Net;

namespace Shoppe.Domain.Exceptions
{
    public enum FilterErrorType
    {
        InvalidSortValue,
        InvalidFilterValue
    }

    public class InvalidFilterException : BaseException
    {
        public override string Title => nameof(InvalidFilterException);
        public FilterErrorType ErrorType { get; }
        public string? FilterField { get; }

        public InvalidFilterException(FilterErrorType errorType, string filterField)
            : base(GenerateMessage(errorType, filterField), HttpStatusCode.BadRequest)
        {
            ErrorType = errorType;
            FilterField = filterField;
        }

        public InvalidFilterException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        public InvalidFilterException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        public InvalidFilterException(string customMessage, FilterErrorType errorType, string filterField)
            : base(GenerateMessage(errorType, filterField, customMessage), HttpStatusCode.BadRequest)
        {
            ErrorType = errorType;
            FilterField = filterField;
        }

        private static string GenerateMessage(FilterErrorType errorType, string filterField, string? customMessage = null)
        {
            return customMessage ?? errorType switch
            {
                FilterErrorType.InvalidSortValue => $"The sorting option '{filterField}' is invalid. Please choose a valid sorting option.",
                FilterErrorType.InvalidFilterValue => $"The filter value '{filterField}' is invalid. Please ensure it meets the required criteria.",
                _ => "An invalid filter parameter was provided. Please check the filter parameters and try again."
            };
        }
    }
}
