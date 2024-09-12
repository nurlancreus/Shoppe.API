using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Auth
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!; 
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
