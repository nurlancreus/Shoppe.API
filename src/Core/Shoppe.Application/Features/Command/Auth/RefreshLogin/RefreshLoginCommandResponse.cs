﻿using Shoppe.Application.DTOs.Token;
using Shoppe.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Auth.RefreshLogin
{
    public class RefreshLoginCommandResponse : AppResponse
    {
        public TokenDTO Token { get; set; } = null!;
    }
}
