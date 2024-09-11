using Shoppe.Application.Responses.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Responses
{
    public class AppResponseWithData<T> : AppResponse where T : class
    {
        public T Data { get; set; } = null!;
    }
}
