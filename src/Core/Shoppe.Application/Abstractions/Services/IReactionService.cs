using Shoppe.Application.DTOs.Reaction;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Replies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface IReactionService
    {
        Task<List<GetReactionDTO>> GetReplyReactionsAsync(Guid id, CancellationToken cancellationToken);
        Task<List<GetReactionDTO>> GetBlogReactionsAsync(Guid id, CancellationToken cancellationToken);
        List<GetReactionDTO> GetBlogReactions(Blog blog);
        List<GetReactionDTO> GetReplyReactions(Reply reply);
        Task ToggleReactionAsync(ToggleReactionDTO toggleReactionDTO, CancellationToken cancellationToken);
    }
}
