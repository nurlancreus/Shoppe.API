using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Storage
{
    public interface IFileUrlGenerator
    {
        string GenerateUrl(string? pathName, string? fileName);
    }
}
