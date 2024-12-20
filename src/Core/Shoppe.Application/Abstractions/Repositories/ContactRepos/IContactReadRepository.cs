﻿using Shoppe.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.ContactRepos
{
    public interface IContactReadRepository : IReadRepository<Contact>
    {
    }
}
