﻿using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.DTOs.Review;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IReplyService
    {
        Task CreateAsync(CreateReplyDTO createReplyDTO, Guid entityId, ReplyType replyType, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateReplyDTO updateReplyTO, CancellationToken cancellationToken);
        Task<GetReplyDTO> GetAsync(Guid replyId, CancellationToken cancellationToken);
        Task<GetAllRepliesDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<List<GetReplyDTO>> GetRepliesByEntityAsync(Guid entityId, ReplyType replyType, CancellationToken cancellationToken);
        Task DeleteAsync(Guid replyId, CancellationToken cancellationToken);
        Task<List<GetReplyDTO>> GetRepliesByParentAsync(Guid parentId, CancellationToken cancellationToken);
        Task<List<GetReplyDTO>> GetRepliesByUserAsync(string userId, CancellationToken cancellationToken);
    }
}
