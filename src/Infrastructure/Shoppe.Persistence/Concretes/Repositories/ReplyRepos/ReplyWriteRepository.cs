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
    public class ReplyWriteRepository : WriteRepository<Reply>, IReplyWriteRepository
    {
        public ReplyWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
