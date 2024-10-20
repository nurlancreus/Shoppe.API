using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.User.Delete
{
    public class DeleteUserCommandRequest : IRequest<DeleteUserCommandResponse>
    {
        public string? UserId { get; set; }
    }
}
