using Shoppe.Application.DTOs.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.User
{
    public record GetUserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<GetImageFileDTO> ProfilePictures { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}
