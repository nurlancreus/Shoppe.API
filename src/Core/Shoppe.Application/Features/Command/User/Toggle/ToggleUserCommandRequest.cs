using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.Deactivate
{
    public class ToggleUserCommandRequest : IRequest<ToggleUserCommandResponse>
    {
        public string? UserId { get; set; }
    }
}
