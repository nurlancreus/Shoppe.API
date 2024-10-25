using Shoppe.Domain.Entities.Reactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.ReactionRepos
{
    public interface IReactionWriteRepository : IWriteRepository<Reaction>
    {
    }
}
