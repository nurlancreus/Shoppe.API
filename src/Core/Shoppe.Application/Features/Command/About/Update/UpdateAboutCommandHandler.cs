using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Shoppe.Application.Abstractions.Repositories.SocialMediaRepos;
using Shoppe.Application.Abstractions.Services.Content;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Application.Features.Command.About.Update
{
    public class UpdateAboutCommandHandler : IRequestHandler<UpdateAboutCommandRequest, UpdateAboutCommandResponse>
    {
        private readonly IAboutReadRepository _aboutReadRepository;
        private readonly ISocialMediaLinkWriteRepository _socialMediaLinkWriteRepository;
        private readonly IContentUpdater _contentUpdater;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAboutCommandHandler(IAboutReadRepository aboutReadRepository, IUnitOfWork unitOfWork, IStorageService storageService, ISocialMediaLinkWriteRepository socialMediaLinkWriteRepository, IContentUpdater contentUpdater)
        {
            _aboutReadRepository = aboutReadRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _socialMediaLinkWriteRepository = socialMediaLinkWriteRepository;
            _contentUpdater = contentUpdater;
        }

        public async Task<UpdateAboutCommandResponse> Handle(UpdateAboutCommandRequest request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var about = await _aboutReadRepository.Table
                .Include(a => a.SocialMediaLinks)
                .SingleOrDefaultAsync(cancellationToken);

            if (about == null) throw new EntityNotFoundException(nameof(about));

            if (!string.IsNullOrEmpty(request.Name) && request.Name != about.Name)
            {
                about.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Title) && request.Title != about.Title)
            {
                about.Title = request.Title;
            }

            if (!string.IsNullOrEmpty(request.Description) && request.Description != about.Description)
            {
                about.Description = request.Description;
            }

            if (!string.IsNullOrEmpty(request.Content) && request.Content != about.Content)
            {

                about.Content = request.Content;
            }

            if (!string.IsNullOrEmpty(request.Email) && request.Email != about.Email)
            {
                // Might add extra logic
                about.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.Phone) && request.Phone != about.Phone)
            {

                if (!IsValidPhone(request.Phone))
                {
                    throw new ValidationException("Invalid phone number format.");
                }

                about.Phone = request.Phone;
            }

            if (request.ContentImages != null && request.ContentImages.Count > 0)
            {

                foreach (var contentImage in request.ContentImages)
                {
                    var (path, fileName) = await _storageService.UploadAsync(AboutConst.ImagesFolder, contentImage.ImageFile);

                    about.ContentImages.Add(new AboutContentImageFile
                    {
                        FileName = fileName,
                        PathName = path,
                        PreviewUrl = contentImage.PreviewUrl,
                        Storage = _storageService.StorageName,
                    });
                }

                about.Content = _contentUpdater.UpdateBlobUrlsInContent(about.Content, about.ContentImages);

            }

            if (_socialMediaLinkWriteRepository.DeleteRange(about.SocialMediaLinks))
            {
                about.SocialMediaLinks.Clear();
            }

            foreach (var link in request.SocialMediaLinks)
            {
                if (!Enum.TryParse<SocialPlatform>(link.SocialPlatform, true, out var parsedPlatform))
                {
                    throw new ValidationException($"Invalid social media platform: {link.SocialPlatform}");
                }

                about.SocialMediaLinks.Add(new SocialMediaLink
                {
                    URL = link.URL,
                    SocialPlatform = parsedPlatform
                });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return new UpdateAboutCommandResponse
            {
                IsSuccess = true,
                Message = "About information updated successfully."
            };

        }

        private bool IsValidPhone(string phone)
        {
            // Use the same validation logic as in the validator
            var regex = new Regex(@"^\+?\d{1,3}?[-.●]?\(?\d{1,4}?\)?[-.●]?\d{1,4}[-.●]?\d{1,9}$");
            return regex.IsMatch(phone);
        }

    }
}
