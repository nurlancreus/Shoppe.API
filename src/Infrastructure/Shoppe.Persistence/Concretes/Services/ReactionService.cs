using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.ReactionRepos;
using Shoppe.Application.Abstractions.Repositories.ReplyRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Reaction;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class ReactionService : IReactionService
    {
        private readonly string[] _blogReactionTypes = Enum.GetNames<BlogReactionType>();
        private readonly string[] _replyReactionTypes = Enum.GetNames<ReplyReactionType>();

        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly IReplyReadRepository _replyReadRepository;
        private readonly IReplyWriteRepository _replyWriteRepository;
        private readonly IReactionReadRepository _reactionReadRepository;
        private readonly IReactionWriteRepository _reactionWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSession _jwtSession;

        public ReactionService(IBlogReadRepository blogReadRepository, IBlogWriteRepository blogWriteRepository, IReplyReadRepository replyReadRepository, IReplyWriteRepository replyWriteRepository, IJwtSession jwtSession, IReactionReadRepository reactionReadRepository, IUnitOfWork unitOfWork, IReactionWriteRepository reactionWriteRepository)
        {
            _blogReadRepository = blogReadRepository;
            _blogWriteRepository = blogWriteRepository;
            _replyReadRepository = replyReadRepository;
            _replyWriteRepository = replyWriteRepository;
            _jwtSession = jwtSession;
            _reactionReadRepository = reactionReadRepository;
            _unitOfWork = unitOfWork;
            _reactionWriteRepository = reactionWriteRepository;
        }

        public async Task<List<GetReactionDTO>> GetBlogReactionsAsync(Guid id, CancellationToken cancellationToken)
        {
            string? userId = null;

            if (_jwtSession.IsAuthenticated())
            {
                userId = _jwtSession.GetUserId();
            }

            var isBlogExist = await _blogReadRepository.IsExistAsync(b => b.Id == id, cancellationToken);

            if (!isBlogExist)
            {
                throw new EntityNotFoundException("Blog is not found");
            }

            List<GetReactionDTO> getBlogReactionDTOs = [];

            foreach (var reactionType in _blogReactionTypes)
            {
                var reactionCount = await _reactionReadRepository.Table.OfType<BlogReaction>().CountAsync(r => r.BlogId == id && r.BlogReactionType == Enum.Parse<BlogReactionType>(reactionType), cancellationToken);

                var isToggled = await _reactionReadRepository.Table.OfType<BlogReaction>().AnyAsync(r => r.BlogId == id && r.UserId == userId && r.BlogReactionType == Enum.Parse<BlogReactionType>(reactionType), cancellationToken);

                var reactionDTO = new GetReactionDTO
                {
                    Id = Guid.NewGuid(),
                    IsToggled = isToggled,
                    ReactionCount = reactionCount,
                    ReactionType = reactionType
                };

                getBlogReactionDTOs.Add(reactionDTO);
            }

            return getBlogReactionDTOs;
        }

        public async Task<List<GetReactionDTO>> GetReplyReactionsAsync(Guid id, CancellationToken cancellationToken)
        {
            string? userId = null;

            if (_jwtSession.IsAuthenticated())
            {
                userId = _jwtSession.GetUserId();
            }

            var isReplyExist = await _replyReadRepository.IsExistAsync(r => r.Id == id, cancellationToken);

            if (!isReplyExist)
            {
                throw new EntityNotFoundException("Reply is not found");
            }

            List<GetReactionDTO> getReplyReactionDTOs = [];

            foreach (var reactionType in _replyReactionTypes)
            {
                var reactionCount = await _reactionReadRepository.Table.OfType<ReplyReaction>().CountAsync(r => r.ReplyId == id && r.ReplyReactionType == Enum.Parse<ReplyReactionType>(reactionType), cancellationToken);

                var isToggled = await _reactionReadRepository.Table.OfType<ReplyReaction>().AnyAsync(r => r.ReplyId == id && r.UserId == userId && r.ReplyReactionType == Enum.Parse<ReplyReactionType>(reactionType), cancellationToken);

                var reactionDTO = new GetReactionDTO
                {
                    Id = Guid.NewGuid(),
                    IsToggled = isToggled,
                    ReactionCount = reactionCount,
                    ReactionType = reactionType
                };

                getReplyReactionDTOs.Add(reactionDTO);
            }

            return getReplyReactionDTOs;
        }

        public List<GetReactionDTO> GetBlogReactions(Blog blog)
        {
            string? userId = null;

            if (_jwtSession.IsAuthenticated())
            {
                userId = _jwtSession.GetUserId();
            }


            List<GetReactionDTO> getBlogReactionDTOs = [];

            foreach (var reactionType in _blogReactionTypes)
            {
                var reactionCount = blog.Reactions.Count(r => r.BlogReactionType == Enum.Parse<BlogReactionType>(reactionType));

                var isToggled = blog.Reactions.Any(r => r.UserId == userId && r.BlogReactionType == Enum.Parse<BlogReactionType>(reactionType));

                var reactionDTO = new GetReactionDTO
                {
                    Id = Guid.NewGuid(),
                    IsToggled = isToggled,
                    ReactionCount = reactionCount,
                    ReactionType = reactionType
                };

                getBlogReactionDTOs.Add(reactionDTO);
            }

            return getBlogReactionDTOs;
        }

        public List<GetReactionDTO> GetReplyReactions(Reply reply)
        {
            string? userId = null;

            if (_jwtSession.IsAuthenticated())
            {
                userId = _jwtSession.GetUserId();
            }


            List<GetReactionDTO> getReplyReactionDTOs = [];

            foreach (var reactionType in _replyReactionTypes)
            {
                var reactionCount = reply.Reactions.Count(r => r.ReplyReactionType == Enum.Parse<ReplyReactionType>(reactionType));

                var isToggled = reply.Reactions.Any(r => r.UserId == userId && r.ReplyReactionType == Enum.Parse<ReplyReactionType>(reactionType));

                var reactionDTO = new GetReactionDTO
                {
                    Id = Guid.NewGuid(),
                    IsToggled = isToggled,
                    ReactionCount = reactionCount,
                    ReactionType = reactionType
                };

                getReplyReactionDTOs.Add(reactionDTO);
            }

            return getReplyReactionDTOs;
        }

        public async Task ToggleReactionAsync(ToggleReactionDTO toggleReactionDTO, CancellationToken cancellationToken)
        {
            var userId = _jwtSession.GetUserId();

            if (toggleReactionDTO.EntityType == ReactionEntityType.Blog)
            {

                var blog = await _blogReadRepository.Table.Include(b => b.Reactions).FirstOrDefaultAsync(b => b.Id == toggleReactionDTO.EntityId, cancellationToken);

                if (blog == null)
                {
                    throw new EntityNotFoundException(nameof(blog));
                }

                var userReaction = blog.Reactions.FirstOrDefault(r => r.UserId == userId);

                if (userReaction is BlogReaction userReact)
                {

                    if (userReaction?.BlogReactionType == toggleReactionDTO.BlogReactionType)
                    {
                        if (blog.Reactions.Remove(userReact))
                        {
                            _reactionWriteRepository.Delete(userReact);
                        }
                    }
                    else
                    {
                        userReact.BlogReactionType = (BlogReactionType)toggleReactionDTO.BlogReactionType!;
                    }
                }
                else
                {

                    var reaction = new BlogReaction
                    {
                        BlogReactionType = (BlogReactionType)toggleReactionDTO.BlogReactionType!,
                        UserId = userId,
                    };

                    blog.Reactions.Add(reaction);
                }

            }
            else if (toggleReactionDTO.EntityType == ReactionEntityType.Reply)
            {
                var reply = await _replyReadRepository.Table.Include(r => r.Reactions).FirstOrDefaultAsync(r => r.Id == toggleReactionDTO.EntityId, cancellationToken);

                if (reply == null)
                {
                    throw new EntityNotFoundException(nameof(reply));
                }

                var userReaction = reply.Reactions.FirstOrDefault(r => r.UserId == userId);

                if (userReaction is ReplyReaction userReact)
                {

                    if (userReaction?.ReplyReactionType == toggleReactionDTO.ReplyReactionType)
                    {
                        if (reply.Reactions.Remove(userReact))
                        {
                            _reactionWriteRepository.Delete(userReact);
                        }
                    }
                    else
                    {
                        userReact.ReplyReactionType = (ReplyReactionType)toggleReactionDTO.ReplyReactionType!;
                    }
                }
                else
                {

                    var reaction = new ReplyReaction
                    {
                        ReplyReactionType = (ReplyReactionType)toggleReactionDTO.ReplyReactionType!,
                        UserId = userId,
                    };

                    reply.Reactions.Add(reaction);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
