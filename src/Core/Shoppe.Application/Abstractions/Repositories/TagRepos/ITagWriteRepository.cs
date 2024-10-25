using Shoppe.Domain.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.TagRepos
{
    public interface ITagWriteRepository : IWriteRepository<Tag>
    {
    }
}
