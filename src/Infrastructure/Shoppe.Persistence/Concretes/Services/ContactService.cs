using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.ContactRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactReadRepository _contactReadRepository;
        private readonly IContactWriteRepository _contactWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;

        public ContactService(IContactReadRepository contactReadRepository, IContactWriteRepository contactWriteRepository, IUnitOfWork unitOfWork, IPaginationService paginationService)
        {
            _contactReadRepository = contactReadRepository;
            _contactWriteRepository = contactWriteRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
        }

        public async Task CreateContactAsync(CreateContactDTO createContactDTO, CancellationToken cancellationToken)
        {
            var contact = new Contact()
            {
                FirstName = createContactDTO.FirstName,
                LastName = createContactDTO.LastName,
                Email = createContactDTO.Email,
                Subject = createContactDTO.Subject,
                Message = createContactDTO.Message,
            };

            bool isAdded = await _contactWriteRepository.AddAsync(contact, cancellationToken);

            if (isAdded)
            {
                throw new AddNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);


        }

        public async Task DeleteContactAsync(string id, CancellationToken cancellationToken)
        {
            var contact = await _contactReadRepository.GetByIdAsync(id, cancellationToken);

            if (contact == null)
            {
                throw new EntityNotFoundException(nameof(contact));
            }

            bool isDeleted = _contactWriteRepository.Delete(contact);

            if (!isDeleted)
            {
                throw new DeleteNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetAllContactsDTO> GetAllContactsAsync(int page, int size, CancellationToken cancellationToken)
        {
            var query = await _contactReadRepository.GetAllAsync(false);

            var (totalItems, _pageSize, _page, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(page, size, query, cancellationToken);

            var contacts = await paginatedQuery.Select(c => c.ToGetContactDTO()).ToListAsync(cancellationToken);

            return new GetAllContactsDTO()
            {
                Page = page,
                PageSize = size,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Contacts = contacts
            };
        }

        public async Task<GetContactDTO> GetContactAsync(string id, CancellationToken cancellationToken)
        {
            var contact = await _contactReadRepository.GetByIdAsync(id, cancellationToken);

            if (contact == null)
            {
                throw new EntityNotFoundException(nameof(contact));
            }

            return contact.ToGetContactDTO();
        }

        public async Task UpdateContactAsync(UpdateContactDTO updateContactDTO, CancellationToken cancellationToken)
        {
            var contact = await _contactReadRepository.GetByIdAsync(updateContactDTO.Id, cancellationToken);

            if (contact == null)
            {
                throw new EntityNotFoundException(nameof(contact));
            }

            if (contact.FirstName != updateContactDTO.FirstName)
            {
                contact.FirstName = updateContactDTO.FirstName;
            }

            if (contact.LastName != updateContactDTO.LastName)
            {
                contact.LastName = updateContactDTO.LastName;
            }

            if (contact.Email != updateContactDTO.Email)
            {
                contact.Email = updateContactDTO.Email;
            }

            if (contact.Subject != updateContactDTO.Subject)
            {
                contact.Subject = updateContactDTO.Subject;
            }

            if (contact.Message != updateContactDTO.Message)
            {
                contact.Message = updateContactDTO.Message;
            }

            bool isUpdated = _contactWriteRepository.Update(contact);

            if (!isUpdated)
            {
                throw new UpdateNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
