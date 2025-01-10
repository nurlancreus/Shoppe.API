using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.ReactionRepos;
using Shoppe.Application.Abstractions.Repositories.ReplyRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Constants;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace Shoppe.Persistence.Concretes.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IReplyReadRepository _replyReadRepository;
        private readonly IReplyWriteRepository _replyWriteRepository;
        private readonly IReactionWriteRepository _reactionWriteRepository;
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IJwtSession _jwtSession;
        private readonly ILogger<ReplyService> _logger;
        private readonly IReactionService _reactionService;

        public ReplyService(
            IReplyReadRepository replyReadRepository,
            IReplyWriteRepository replyWriteRepository,
            IUnitOfWork unitOfWork,
            IPaginationService paginationService,
            IJwtSession jwtSession,
            IBlogReadRepository blogReadRepository,
            ILogger<ReplyService> logger,
            IReactionService reactionService,
            IReactionWriteRepository reactionWriteRepository)
        {
            _replyReadRepository = replyReadRepository;
            _replyWriteRepository = replyWriteRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _jwtSession = jwtSession;
            _blogReadRepository = blogReadRepository;
            _logger = logger;
            _reactionService = reactionService;
            _reactionWriteRepository = reactionWriteRepository;
        }

        public async Task CreateAsync(CreateReplyDTO createReplyDTO, Guid entityId, ReplyType replyType, CancellationToken cancellationToken)
        {
            var reply = await CreateReplyByTypeAsync(entityId, replyType, cancellationToken);
            reply.Body = createReplyDTO.Body;
            reply.ReplierId = _jwtSession.GetUserId();

            if (!await _replyWriteRepository.AddAsync(reply, cancellationToken))
            {
                _logger.LogWarning("Failed to add reply for entity ID {EntityId}", entityId);
                throw new AddNotSucceedException("Cannot add new reply");
            }

            _logger.LogInformation("Reply added successfully for entity ID {EntityId}", entityId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid replyId, CancellationToken cancellationToken)
        {
            var reply = await GetAndValidateReplyAsync(replyId, cancellationToken);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!_replyWriteRepository.RecursiveDelete(reply))
            {
                _logger.LogWarning("Failed to delete reply with ID {ReplyId}", replyId);
                throw new DeleteNotSucceedException("Cannot delete the reply");
            }

            _logger.LogInformation("Reply deleted successfully with ID {ReplyId}", replyId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        //private bool RecursiveDelete(Reply parent)
        //{
        //    if (parent.Children != null && parent.Children.Count != 0)
        //    {
        //        // Recursively delete children
        //        foreach (var child in parent.Children)
        //        {
        //            RecursiveDelete(child);
        //        }
        //    }

        //    return _replyWriteRepository.Delete(parent) && _reactionWriteRepository.DeleteRange(parent.Reactions);
        //}

        public async Task<GetAllRepliesDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var replyQuery = _replyReadRepository.Table.AsQueryable().AsNoTracking();
            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, replyQuery, cancellationToken);
            var replies = await paginationResult.PaginatedQuery
                .ToListAsync(cancellationToken);

            var repliesDto = replies.Select(r => r.ToGetReplyDTO(_reactionService)).ToList();

            return new GetAllRepliesDTO
            {
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages,
                Replies = repliesDto,
            };
        }

        public async Task<GetReplyDTO> GetAsync(Guid replyId, CancellationToken cancellationToken)
        {
            var reply = await _replyReadRepository.Table
                .FirstOrDefaultAsync(r => r.Id == replyId, cancellationToken);

            if (reply == null)
            {
                _logger.LogWarning("Reply with ID {ReplyId} not found", replyId);
                throw new EntityNotFoundException("Reply not found");
            }

            return reply.ToGetReplyDTO(_reactionService);
        }

        public async Task<List<GetReplyDTO>> GetRepliesByEntityAsync(Guid entityId, ReplyType replyType, CancellationToken cancellationToken)
        {

            var replyQuery = replyType switch
            {
                ReplyType.Blog => _replyReadRepository.Table.OfType<BlogReply>()
                    .Where(r => r.BlogId == entityId)
                    .AsNoTracking(),

                ReplyType.Reply => _replyReadRepository.Table.OfType<Reply>()
                    .Where(r => r.ParentId == entityId)
                    .AsNoTracking(),
                _ => throw new InvalidOperationException("Invalid reply type"),
            };

            var replies = await replyQuery.OrderBy(r => r.CreatedAt).ToListAsync(cancellationToken);

            return replies.Select(r => r.ToGetReplyDTO(_reactionService)).ToList();
        }

        public async Task<List<GetReplyDTO>> GetRepliesByParentAsync(Guid parentId, CancellationToken cancellationToken)
        {
            var replies = await _replyReadRepository.Table
                .Where(r => r.ParentId == parentId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return replies.Select(r => r.ToGetReplyDTO(_reactionService)).ToList();
        }

        public async Task UpdateAsync(UpdateReplyDTO updateReplyDTO, CancellationToken cancellationToken)
        {
            var reply = await GetAndValidateReplyAsync(updateReplyDTO.Id, cancellationToken);

            if (updateReplyDTO.Body is string newBody && reply.Body != newBody)
            {
                reply.Body = newBody;
                _logger.LogInformation("Updated reply body for reply ID {ReplyId}", updateReplyDTO.Id);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Reply> CreateReplyByTypeAsync(Guid entityId, ReplyType replyType, CancellationToken cancellationToken)
        {
            return replyType switch
            {
                ReplyType.Blog => await CreateBlogReplyAsync(entityId, cancellationToken),
                ReplyType.Reply => await CreateChildReplyAsync(entityId, cancellationToken),
                _ => throw new AddNotSucceedException("Invalid reply type"),
            };
        }

        private async Task<Reply> CreateBlogReplyAsync(Guid entityId, CancellationToken cancellationToken)
        {
            if (!await _blogReadRepository.IsExistAsync(b => b.Id == entityId, cancellationToken))
            {
                _logger.LogWarning("Blog with ID {EntityId} not found", entityId);
                throw new EntityNotFoundException($"Blog with ID {entityId} not found");
            }

            return new BlogReply { BlogId = entityId, Depth = 0 };
        }

        private async Task<Reply> CreateChildReplyAsync(Guid entityId, CancellationToken cancellationToken)
        {
            var parentReply = await _replyReadRepository.GetByIdAsync(entityId, cancellationToken, false);

            if (parentReply == null)
            {
                _logger.LogWarning("Reply with ID {EntityId} not found", entityId);
                throw new EntityNotFoundException($"Reply with ID {entityId} not found");
            }

            var newDepth = Math.Min(ReplyConst.MaxDepthLevel, (byte)(parentReply.Depth + 1));

            return new Reply { ParentId = entityId, Depth = newDepth };
        }

        private async Task<Reply> GetAndValidateReplyAsync(Guid replyId, CancellationToken cancellationToken)
        {
            var reply = await _replyReadRepository.Table
                .Include(r => r.Replier)
                .FirstOrDefaultAsync(r => r.Id == replyId, cancellationToken);

            if (reply == null)
            {
                _logger.LogWarning("Reply with ID {ReplyId} not found", replyId);
                throw new EntityNotFoundException("Reply not found");
            }

            if (!_jwtSession.IsAdmin() && _jwtSession.GetUserId() != reply.ReplierId)
            {
                _logger.LogWarning("Unauthorized access attempt for reply ID {ReplyId}", replyId);
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
            }

            return reply;
        }


        public async Task<List<GetReplyDTO>> GetRepliesByUserAsync(string userId, CancellationToken cancellationToken)
        {
            var replies = await _replyReadRepository.Table
                .Where(r => r.Replier.Id == userId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return replies.Select(r => r.ToGetReplyDTO(_reactionService)).ToList();
        }
    }
}
