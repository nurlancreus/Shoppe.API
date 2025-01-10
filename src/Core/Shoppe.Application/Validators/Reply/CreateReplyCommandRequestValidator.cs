using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos; 
using Shoppe.Application.Abstractions.Repositories.ReplyRepos;
using Shoppe.Domain.Constants;
using Shoppe.Application.Features.Command.Reply.Create;
using Shoppe.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Application.Validators.Reply
{
    public class CreateReplyCommandRequestValidator : AbstractValidator<CreateReplyCommandRequest>
    {
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IReplyReadRepository _replyReadRepository;

        public CreateReplyCommandRequestValidator(IBlogReadRepository blogReadRepository, IReplyReadRepository replyReadRepository)
        {
            _blogReadRepository = blogReadRepository;
            _replyReadRepository = replyReadRepository;

            RuleFor(x => x.Body)
                .MaximumLength(ReplyConst.MaxBodyLength) 
                .WithMessage($"Body cannot exceed {ReplyConst.MaxBodyLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Body));

            RuleFor(x => x.EntityId)
                .NotEmpty().WithMessage("Entity ID is required.")
                .MustAsync(ValidateEntityIdAsync).WithMessage("Entity not found.");
        }

        private async Task<bool> ValidateEntityIdAsync(CreateReplyCommandRequest request, Guid? entityId, CancellationToken cancellationToken)
        {
            if (request.Type == ReplyType.Blog)
            {
                return await _blogReadRepository.IsExistAsync(b => b.Id == entityId, cancellationToken);
            }
            else if (request.Type == ReplyType.Reply)
            {
                return await _replyReadRepository.IsExistAsync(b => b.Id == entityId, cancellationToken);
            }

            return false;
        }
    }
}
