using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Shoppe.Application.Abstractions.Services.Content;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.DTOs.About;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.SocialMediaLink;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.About.Get
{
    public class GetAboutQueryHandler : IRequestHandler<GetAboutQueryRequest, GetAboutQueryResponse>
    {
        private readonly IAboutReadRepository _aboutReadRepository;

        public GetAboutQueryHandler(IAboutReadRepository aboutReadRepository)
        {
            _aboutReadRepository = aboutReadRepository;
        }

        public async Task<GetAboutQueryResponse> Handle(GetAboutQueryRequest request, CancellationToken cancellationToken)
        {
            var about = await _aboutReadRepository.Table
                                            .Include(a => a.SocialMediaLinks)
                                            .Include(a => a.ContentImages)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(cancellationToken);

            if (about == null) throw new EntityNotFoundException(nameof(about));

            var getAboutDto = new GetAboutDTO
            {
                Name = about.Name,
                Description = about.Description,
                Title = about.Title,
                Email = about.Email,
                Phone = about.Phone,
                Content = about.Content,
                ContentImages = about.ContentImages.Select(i => i.ToGetContentFileDTO()).ToList(),
                SocialMediaLinks = about.SocialMediaLinks.Select(s => new GetSocialMediaLinkDTO
                {
                    Id = s.Id,
                    SocialPlatform = s.SocialPlatform.ToString(),
                    URL = s.URL,
                    CreatedAt = s.CreatedAt,
                }).ToList(),

            };

            return new GetAboutQueryResponse
            {
                IsSuccess = true,
                Data = getAboutDto
            };
        }
    }
}
