using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Tag.Create
{
    public class CreateTagCommandRequest : IRequest<CreateTagCommandResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Type { get; set; } = "Blog";
    }
}
