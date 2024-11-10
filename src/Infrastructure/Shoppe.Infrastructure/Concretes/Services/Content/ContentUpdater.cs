using Shoppe.Application.Abstractions.Services.Content;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Content
{
    public class ContentUpdater : IContentUpdater
    {
        private readonly IFileUrlGenerator _fileUrlGenerator;

        public ContentUpdater(IFileUrlGenerator fileUrlGenerator)
        {
            _fileUrlGenerator = fileUrlGenerator;
        }

        public string? UpdateBlobUrlsInContent<T>(string? content, ICollection<T> contentImages) where T : ContentImageFile
        {
            if (content == null) return null;

            string pattern = @"<img[^>]+src=""(blob:[^""]+)""[^>]*>";

            Regex regex = new(pattern, RegexOptions.IgnoreCase);
            var matches = regex.Matches(content);

            foreach (Match match in matches)
            {
                var blobUrl = match.Groups[1].Value;

                var matchingImage = FindMatchingImage(blobUrl, contentImages);
                if (matchingImage != null)
                {
                    string generatedUrl = _fileUrlGenerator.GenerateUrl(matchingImage.PathName, matchingImage.FileName);

                    content = content.Replace(blobUrl, generatedUrl);
                }
            }

            return content;
        }

        private static ContentImageFile? FindMatchingImage<T>(string blobUrl, ICollection<T> contentImages) where T : ContentImageFile
        {
            foreach (var image in contentImages)
            {
                if (image.PreviewUrl == blobUrl)
                {
                    return image;
                }
            }
            return null;
        }
    }
}
