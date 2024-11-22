using Shoppe.Application.Abstractions.Repositories.ReactionRepos;
using Shoppe.Application.Abstractions.Repositories.ReplyRepos;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.ReplyRepos
{
    public class ReplyWriteRepository : SelfReferencedWriteRepository<Reply>, IReplyWriteRepository
    {
        private readonly IReactionWriteRepository _reactionWriteRepository;
        public ReplyWriteRepository(ShoppeDbContext context, IReactionWriteRepository reactionWriteRepository) : base(context)
        {
            _reactionWriteRepository = reactionWriteRepository;
        }

        protected override void PerformExtraDeleteOperations(Reply parent)
        {
            if (parent.Reactions != null && parent.Reactions.Count > 0)
            {
                _reactionWriteRepository.DeleteRange(parent.Reactions);
            }
        }
    }
}
