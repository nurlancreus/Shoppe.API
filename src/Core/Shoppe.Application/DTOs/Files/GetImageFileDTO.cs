﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Files
{
    public record GetImageFileDTO : GetFileDTO
    {
    
        public bool IsMain { get; set; }
    }
}
