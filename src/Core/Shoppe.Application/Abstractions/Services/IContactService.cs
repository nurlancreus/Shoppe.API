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
        Task CreateAsync(CreateContactDTO createContactDTO, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateContactDTO updateContactDTO, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task AnswerContactMessageAsync(AnswerContactMessageDTO answerContactMessageDTO, CancellationToken cancellationToken);
        Task<GetAllContactsDTO> GetAllAsync(int page, int size, CancellationToken cancellationToken);
        Task<GetContactDTO> GetAsync(Guid id, CancellationToken cancellationToken);
    }
}
