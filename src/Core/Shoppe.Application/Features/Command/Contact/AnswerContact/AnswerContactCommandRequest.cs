﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Contact.AnswerContact
{
    public class AnswerContactCommandRequest : IRequest<AnswerContactCommandResponse>
    {
        public Guid? ContactId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}