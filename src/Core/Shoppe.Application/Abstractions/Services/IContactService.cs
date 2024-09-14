using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.DTOs.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IContactService
    {
        Task CreateContactAsync(CreateContactDTO createContactDTO, CancellationToken cancellationToken);
        Task UpdateContactAsync(UpdateContactDTO updateContactDTO, CancellationToken cancellationToken);
        Task DeleteContactAsync(string id, CancellationToken cancellationToken);
        Task<GetAllContactsDTO> GetAllContactsAsync(int page, int size, CancellationToken cancellationToken);
        Task<GetContactDTO> GetContactAsync(string id, CancellationToken cancellationToken);
    }
}
