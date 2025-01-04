using Microsoft.EntityFrameworkCore.Infrastructure;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Replies
{
    public class Reply : BaseEntity, ISelfReferenced<Reply>
    {
        private readonly ILazyLoader _lazyLoader;

        public Reply()
        {
            
        }

        public Reply(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private ApplicationUser _replier = null!;
        private ICollection<Reply> _children = [];
        private ICollection<ReplyReaction> _reactions = [];
        public string? Body { get; set; } = null!;
        public string Type { get; set; }

        public byte Depth { get; set; }


        [ForeignKey(nameof(Replier))]
        public string ReplierId { get; set; } = string.Empty;
        public ApplicationUser Replier {
            get => _lazyLoader.Load(this, ref _replier!)!;
            set => _replier = value;
        }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }
        public Reply? Parent { get; set; }

        public ICollection<Reply> Children 
        {
            get => _lazyLoader.Load(this, ref _children) ?? [];
            set => _children = value;
        }

        public ICollection<ReplyReaction> Reactions
        {
            get => _lazyLoader.Load(this, ref _reactions) ?? [];
            set => _reactions = value;
        }

    }
}
