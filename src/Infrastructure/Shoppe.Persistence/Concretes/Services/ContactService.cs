using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.ContactRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Mail;
using Shoppe.Application.Abstractions.Services.Mail.Templates;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Contact;
using Shoppe.Application.DTOs.Mail;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Contacts;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactReadRepository _contactReadRepository;
        private readonly IContactWriteRepository _contactWriteRepository;
        private readonly IJwtSession _jwtSession;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IEmailService _emailService;
        private readonly IContactEmailTemplateService _emailTemplateService;

        public ContactService(IContactReadRepository contactReadRepository, IContactWriteRepository contactWriteRepository, IUnitOfWork unitOfWork, IPaginationService paginationService, IJwtSession jwtSession, IEmailService emailService, IContactEmailTemplateService emailTemplateService)
        {
            _contactReadRepository = contactReadRepository;
            _contactWriteRepository = contactWriteRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _jwtSession = jwtSession;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
        }

        public async Task AnswerContactMessageAsync(AnswerContactMessageDTO answerContactMessageDTO, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _jwtSession.ValidateAdminAccess();

            var contact = await _contactReadRepository.Table.Include(c => (c as RegisteredContact)!.User).FirstOrDefaultAsync(c => c.Id == answerContactMessageDTO.ContactId, cancellationToken);
            
            if (contact == null)
            {
                throw new EntityNotFoundException(nameof(contact));
            }

            if (!string.IsNullOrEmpty(answerContactMessageDTO.Message))
            {

                if (contact is RegisteredContact registeredContact && registeredContact.User != null)
                {
                    var name = $"{registeredContact.User.FirstName} {registeredContact.User.LastName}";
                    var emailBody = _emailTemplateService.GenerateContactResponseTemplate(name, contact.Subject, answerContactMessageDTO.Message);

                    await _emailService.SendEmailAsync(new RecipientDetailsDTO
                    {
                        Name = $"{registeredContact.User.FirstName} {registeredContact.User.LastName}",
                        Email = registeredContact.User.Email!,
                    }, contact.Subject.ToString(), emailBody);

                }
                else if (contact is UnRegisteredContact unRegisteredContact)
                {
                    var name = $"{unRegisteredContact.FirstName} {unRegisteredContact.LastName}";
                    var emailBody = _emailTemplateService.GenerateContactResponseTemplate(name, contact.Subject, answerContactMessageDTO.Message);

                    await _emailService.SendEmailAsync(new RecipientDetailsDTO
                    {
                        Name = name,
                        Email = unRegisteredContact.Email,
                    }, contact.Subject.ToString(), emailBody);
                }

                contact.IsAnswered = true;
                contact.AnsweredAt = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task CreateAsync(CreateContactDTO createContactDTO, CancellationToken cancellationToken)
        {
            Contact contact = new();
            string? recipientName = null;
            string? recipientEmail = null;

            if (_jwtSession.IsAuthenticated())
            {
                var user = _jwtSession.GetUser();

                contact = new RegisteredContact
                {
                    UserId = user.Id,
                };

                recipientName = $"{user.FirstName} {user.LastName}";
                recipientEmail = user.Email;
            }

            else if (!string.IsNullOrEmpty(createContactDTO.FirstName) && !string.IsNullOrEmpty(createContactDTO.LastName) && !string.IsNullOrEmpty(createContactDTO.Email))
            {
                contact = new UnRegisteredContact
                {
                    FirstName = createContactDTO.FirstName,
                    LastName = createContactDTO.LastName,
                    Email = createContactDTO.Email,
                };

                recipientName = $"{createContactDTO.FirstName} {createContactDTO.LastName}";
                recipientEmail = createContactDTO.Email;

            }

            contact.Subject = createContactDTO.Subject;
            contact.Message = createContactDTO.Message;

            bool isAdded = await _contactWriteRepository.AddAsync(contact, cancellationToken);

            if (!isAdded)
            {
                throw new AddNotSucceedException();
            }


            var emailBody = _emailTemplateService.GenerateContactReceivedTemplate(recipientName!, contact.Subject);

            await _emailService.SendEmailAsync(new RecipientDetailsDTO
            {
                Name = recipientName!,
                Email = recipientEmail!,
            }, contact.Subject.ToString(), emailBody);


            await _unitOfWork.SaveChangesAsync(cancellationToken);


        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

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

        public async Task<GetAllContactsDTO> GetAllAsync(int page, int size, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            var query = _contactReadRepository.Table.Include(c => (c as RegisteredContact)!.User).AsNoTracking();

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

        public async Task<GetContactDTO> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            var contact = await _contactReadRepository.Table.Include(c => (c as RegisteredContact)!.User).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (contact == null)
            {
                throw new EntityNotFoundException(nameof(contact));
            }

            return contact.ToGetContactDTO();
        }

        public async Task UpdateAsync(UpdateContactDTO updateContactDTO, CancellationToken cancellationToken)
        {
            var contact = await _contactReadRepository.GetByIdAsync(updateContactDTO.Id, cancellationToken);

            if (contact == null)
            {
                throw new EntityNotFoundException(nameof(contact));
            }

            if (contact is UnRegisteredContact unregisteredContact)
            {

                if (updateContactDTO.Email != null && unregisteredContact.Email != updateContactDTO.Email)
                {
                    unregisteredContact.Email = updateContactDTO.Email;
                }
            }


            if (updateContactDTO.Subject is ContactSubject contactSubject && contact.Subject != contactSubject)
            {
                contact.Subject = contactSubject;
            }

            if (updateContactDTO.Message != null && contact.Message != updateContactDTO.Message)
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
