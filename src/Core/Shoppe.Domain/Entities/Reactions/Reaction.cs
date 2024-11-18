using Microsoft.EntityFrameworkCore.Infrastructure;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Reactions
{
    public class Reaction : BaseEntity
    {
        private readonly ILazyLoader _lazyLoader;

        public Reaction()
        {

        }

        public Reaction(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private ApplicationUser _user = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser User
        {
            get => _lazyLoader.Load(this, ref _user!)!;
            set => _user = value;
        }

        public string EntityType { get; set; } 

    }
}
