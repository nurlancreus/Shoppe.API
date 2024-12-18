using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Options.Storage.AWS
{
    public class AWSOptions
    {
        public string AccessKey { get; set; } = string.Empty;
        public string SecretAccessKey { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public S3Options AWSS3 { get; set; } = null!;
    }
}
