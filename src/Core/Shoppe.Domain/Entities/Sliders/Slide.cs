﻿using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Sliders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Slide : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string URL { get; set; } = null!;
        public string ButtonText { get; set; } = null!;
        public SlideImageFile SlideImageFile { get; set; } = null!;
        public Guid SliderId { get; set; }
        public Slider Slider { get; set; } = null!;
        public byte Order { get; set; }
    }
}
