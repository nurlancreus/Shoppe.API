using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Content
{
    public interface IContentUpdater
    {
        string? UpdateBlobUrlsInContent<T>(string? content, ICollection<T> contentImages) where T : ContentImageFile;

    }
}
