using Shoppe.Application.Abstractions.Repositories;
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
    public class ReplyReadRepository : ReadRepository<Reply>, IReplyReadRepository
    {
        public ReplyReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
