﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Discount
{
    public record UpdateDiscountDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
