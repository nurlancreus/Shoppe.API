using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.DTOs
{
    public record UpdateApiClientRequestDTO
    {
        public Guid Id { get; set; }  // Required for identifying the client to update
        public string? CompanyName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? State { get; set; }
    }
}
