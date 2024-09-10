using Shoppe.Application.Options.Storage.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Options.Storage
{
    public class StorageOptions
    {
        public AWSOptions AWS { get; set; } = null!;
    }
}
