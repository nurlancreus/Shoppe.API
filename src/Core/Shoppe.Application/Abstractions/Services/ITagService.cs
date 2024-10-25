using Shoppe.Application.DTOs.Tag;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface ITagService
    {
        Task CreateAsync(CreateTagDTO createTagDTO, CancellationToken cancellationToken);
        Task<GetTagDTO> GetAsync(string id, CancellationToken cancellationToken);
        Task<GetAllTagsDTO> GetAllAsync(int page, int pageSize, TagType? type, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateTagDTO updateTagDTO, CancellationToken cancellationToken);
        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
