using Shoppe.Domain.Entities.Replies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.ReplyRepos
{
    public interface IReplyWriteRepository : ISelfReferencedWriteRepository<Reply>
    {
    }
}
