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
        public string? Body { get; set; } = null!;
        public string Type { get; set; } = string.Empty;


        [ForeignKey(nameof(Replier))]
        public string ReplierId { get; set; } = string.Empty;
        public ApplicationUser Replier { get; set; } = null!;

        [ForeignKey(nameof(ParentReply))]
        public Guid? ParentReplyId { get; set; }
        public Reply? ParentReply { get; set; }

        public ICollection<Reply> Replies { get; set; } = [];
        public ICollection<ReplyReaction> Reactions { get; set; } = [];

    }
}
