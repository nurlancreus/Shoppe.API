﻿using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
       public DbSet<T> Table { get; }
    }
}