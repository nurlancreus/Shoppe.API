﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Files
{
    public abstract class ImageFile : ApplicationFile
    {
        public bool IsMain { get; set; }
    }
}
