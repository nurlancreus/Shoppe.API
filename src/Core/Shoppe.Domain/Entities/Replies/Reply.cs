using Microsoft.EntityFrameworkCore.Infrastructure;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Replies
{
    public class Reply : BaseEntity
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
        private ICollection<Reply> _replies = [];
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

        [ForeignKey(nameof(ParentReply))]
        public Guid? ParentReplyId { get; set; }
        public Reply? ParentReply { get; set; }

        public ICollection<Reply> Replies 
        {
            get => _lazyLoader.Load(this, ref _replies) ?? [];
            set => _replies = value;
        }

        public ICollection<ReplyReaction> Reactions
        {
            get => _lazyLoader.Load(this, ref _reactions) ?? [];
            set => _reactions = value;
        }

    }
}
