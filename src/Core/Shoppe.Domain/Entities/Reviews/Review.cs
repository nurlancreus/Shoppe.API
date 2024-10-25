using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Reviews
{
    public class Review : BaseEntity
    {
        public string? Body { get; set; } = null!;
        public Rating Rating { get; set; }
        public string Type { get; set; } = string.Empty;


        [ForeignKey(nameof(Reviewer))]
        public string ReviewerId { get; set; } = string.Empty;
        public ApplicationUser Reviewer { get; set; } = null!;
    }
}
