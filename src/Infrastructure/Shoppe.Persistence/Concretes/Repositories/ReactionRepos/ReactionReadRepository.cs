using Shoppe.Application.Abstractions.Repositories.ReactionRepos;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.ReactionRepos
{
    public class ReactionReadRepository : ReadRepository<Reaction>, IReactionReadRepository
    {
        public ReactionReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
