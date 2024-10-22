using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Shoppe.Application.DTOs.About;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Section;
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
                                            .Include(a => a.Sections)
                                                .ThenInclude(s => s.SectionImageFiles)
                                            .Include(a => a.SocialMediaLinks)
                                            .AsNoTracking()
                                            .SingleOrDefaultAsync(cancellationToken);

            if (about == null) throw new EntityNotFoundException(nameof(about));

            return new GetAboutQueryResponse
            {
                IsSuccess = true,
                Data = new GetAboutDTO
                {
                    Name = about.Name,
                    Description = about.Description,
                    Title = about.Title,
                    Email = about.Email,
                    Phone = about.Phone,
                    Sections = about.Sections.Select(s => new GetSectionDTO
                    {
                        Id = s.Id.ToString(),
                        Title = s.Title,
                        SectionImageFiles = s.SectionImageFiles.Select(si => new GetImageFileDTO
                        {
                            Id = si.Id.ToString(),
                            FileName = si.FileName,
                            PathName = si.PathName,
                            IsMain = si.IsMain,
                            CreatedAt = si.CreatedAt
                        }).ToList(),
                        Description = s.Description,
                        CreatedAt = s.CreatedAt
                    }).ToList()
                }
            };
        }
    }
}
