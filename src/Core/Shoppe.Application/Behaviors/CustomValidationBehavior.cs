using FluentValidation.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Behaviors
{
    public sealed class CustomValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public CustomValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Create the ValidationContext using the request object
            var context = new ValidationContext<TRequest>(request);

            // Run all validators asynchronously and gather their results
            var validationResults = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

            // Collect all validation failures
            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            // If there are any failures, throw a ValidationException with custom error messages
            if (failures.Any())
            {
                var validationFailures = failures
                    .Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage))
                    .ToList();

                throw new ValidationException(validationFailures);
            }

            // Proceed to the next behavior in the pipeline
            return await next();
        }
    }
}
