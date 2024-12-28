using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities
{
    public class Address : BaseEntity
    {

        public string FirstName {  get; set; } = string.Empty;
        public string LastName {  get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string AddressType {  get; set; }
        public ICollection<Order> Orders { get; set; } = [];
    }
}
