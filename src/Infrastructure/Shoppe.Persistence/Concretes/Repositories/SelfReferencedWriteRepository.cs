using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Flags;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories
{
    public class SelfReferencedWriteRepository<T> : WriteRepository<T>, ISelfReferencedWriteRepository<T>
    where T : BaseEntity, ISelfReferenced<T>
    {
        public SelfReferencedWriteRepository(ShoppeDbContext context) : base(context) { }

        public virtual bool RecursiveDelete(T parent)
        {
            // Delete children recursively
            if (parent.Children != null && parent.Children.Count > 0)
            {
                foreach (var child in parent.Children)
                {
                    RecursiveDelete(child);
                }
            }

            // Call hook for extra operations
            PerformExtraDeleteOperations(parent);

            // Delete the parent entity
            var entry = Table.Remove(parent);
            return entry.State == EntityState.Deleted;
        }

        /// <summary>
        /// Hook method for additional delete logic, can be overridden.
        /// </summary>
        /// <param name="parent">The parent entity being deleted.</param>
        protected virtual void PerformExtraDeleteOperations(T parent)
        {
            // No additional operations by default.
        }
    }

}
