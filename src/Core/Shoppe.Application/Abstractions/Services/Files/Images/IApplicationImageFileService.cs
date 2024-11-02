using Shoppe.Application.DTOs.Files;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Files.Images
{
    public interface IApplicationImageFileService
    {
        Task<GetAllImagesDTO> GetAllImagesDTO (int page, int pageSize, ImageFileType? type, CancellationToken cancellationToken);
    }
}
