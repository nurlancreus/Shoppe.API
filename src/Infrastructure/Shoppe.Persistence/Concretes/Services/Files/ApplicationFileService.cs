using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.FileRepos;
using Shoppe.Application.Abstractions.Services.Files;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services.Files
{
    public class ApplicationFileService : IApplicationFileService
    {
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IPaginationService _paginationService;

        public ApplicationFileService(IFileReadRepository fileReadRepository, IPaginationService paginationService)
        {
            _fileReadRepository = fileReadRepository;
            _paginationService = paginationService;
        }

        public async Task<GetAllImagesDTO> GetAllImagesDTO(int page, int pageSize, ImageFileType? type, CancellationToken cancellationToken)
        {
            IQueryable<ApplicationFile>? query = null;

            //if (type == null) query = _fileReadRepository.Table.AsNoTracking().Where(f => EF.Property<string>(f, "discriminator").Contains("Image"));
            if (type == null) query = _fileReadRepository.Table.AsNoTracking().OfType<ImageFile>();
            else if (type == ImageFileType.Blog) query = _fileReadRepository.Table.AsNoTracking().OfType<BlogImageFile>();
            else if (type == ImageFileType.Product) query = _fileReadRepository.Table.AsNoTracking().OfType<ProductImageFile>();
            else if (type == ImageFileType.Slider) query = _fileReadRepository.Table.AsNoTracking().OfType<SlideImageFile>();
            else if (type == ImageFileType.User) query = _fileReadRepository.Table.AsNoTracking().OfType<UserProfileImageFile>();
            else throw new ArgumentException("Invalid image file type");

            if (query is not IQueryable<ImageFile> imagesQuery) throw new InvalidCastException("Invalid image file type");

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, imagesQuery, cancellationToken);

            var imageDtos = await paginationResult.PaginatedQuery.Select(i => i.ToGetImageFileDTO()).ToListAsync(cancellationToken);

            return new GetAllImagesDTO
            {
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages,
                Images = imageDtos
            };
        }
    }
}
