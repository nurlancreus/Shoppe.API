using Azure;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities.Tags;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class TagService : ITagService
    {
        private readonly ITagReadRepository _tagReadRepository;
        private readonly ITagWriteRepository _tagWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;

        public TagService(ITagReadRepository tagReadRepository, ITagWriteRepository tagWriteRepository, IUnitOfWork unitOfWork, IPaginationService paginationService)
        {
            _tagReadRepository = tagReadRepository;
            _tagWriteRepository = tagWriteRepository;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
        }

        public async Task CreateAsync(CreateTagDTO createTagDTO, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var existedTag = await _tagReadRepository.GetAsync(t => t.Name == createTagDTO.Name, cancellationToken, false);

            if (existedTag != null)
            {
                throw new AddNotSucceedException("Tag already exists.");
            }

            Tag? tag = null;

            if (createTagDTO.Type == TagType.Blog)
            {

                tag = new BlogTag
                {
                    Name = createTagDTO.Name,
                    Description = createTagDTO.Description
                };
            }
            else throw new AddNotSucceedException("Invalid tag type");

            await _tagWriteRepository.AddAsync(tag, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var tag = await _tagReadRepository.GetByIdAsync(id, cancellationToken);

            if (tag == null)
            {
                throw new EntityNotFoundException(nameof(tag));
            }

            bool isDeleted = _tagWriteRepository.Delete(tag);

            if (!isDeleted)
            {
                throw new DeleteNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }

        public async Task<GetAllTagsDTO> GetAllAsync(int page, int pageSize, TagType? type, CancellationToken cancellationToken)
        {
            IQueryable<Tag>? query = null;

            if (type != null)
            {
                query = _tagReadRepository.Table.Where(t => t.Type == type.Value.ToString()).AsNoTrackingWithIdentityResolution();
            }
            else
            {
                query = await _tagReadRepository.GetAllAsync(false);
            }

            if (query == null)
            {
                throw new InvalidOperationException("Query not found, operation invalid");
            }

            var (totalItems, _pageSize, _page, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(page, pageSize, query, cancellationToken);

            var tags = await paginatedQuery.Select(tag => new GetTagDTO
            {
                Id = tag.Id.ToString(),
                Name = tag.Name,
                Description = tag.Description,
                Type = tag.Type,
            }).ToListAsync(cancellationToken);

            return new GetAllTagsDTO
            {
                Tags = tags,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<GetTagDTO> GetAsync(string id, CancellationToken cancellationToken)
        {
            var tag = await _tagReadRepository.GetByIdAsync(id, cancellationToken, false);

            if (tag == null)
            {
                throw new EntityNotFoundException(nameof(tag));
            }

            return new GetTagDTO
            {
                Id = tag.Id.ToString(),
                Name = tag.Name,
                Description = tag.Description,
                Type = tag.Type,
            };
        }

        public async Task UpdateAsync(UpdateTagDTO updateTagDTO, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var tag = await _tagReadRepository.GetByIdAsync(updateTagDTO.Id, cancellationToken);

            if (tag == null)
            {
                throw new EntityNotFoundException(nameof(tag));
            }

            if (!string.IsNullOrWhiteSpace(updateTagDTO.Name) && tag.Name.ToLower() != updateTagDTO.Name.ToLower())
            {
                tag.Name = updateTagDTO.Name;
            }

            if (!string.IsNullOrWhiteSpace(updateTagDTO.Description) && tag.Description?.ToLower() != updateTagDTO.Description.ToLower())
            {
                tag.Description = updateTagDTO.Description;
            }

            var isUpdated = _tagWriteRepository.Update(tag);

            if (!isUpdated)
            {
                throw new UpdateNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }
    }
}
