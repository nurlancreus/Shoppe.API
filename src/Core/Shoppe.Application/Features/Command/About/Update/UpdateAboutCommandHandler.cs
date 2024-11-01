using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Shoppe.Application.Abstractions.Repositories.SectionRepos;
using Shoppe.Application.Abstractions.Repositories.SocialMediaRepos;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Sections;
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
        private readonly ISectionWriteRepository _sectionWriteRepository;
        private readonly ISocialMediaLinkWriteRepository _socialMediaLinkWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAboutCommandHandler(IAboutReadRepository aboutReadRepository, IUnitOfWork unitOfWork, IStorageService storageService, ISectionWriteRepository sectionWriteRepository, ISocialMediaLinkWriteRepository socialMediaLinkWriteRepository)
        {
            _aboutReadRepository = aboutReadRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _sectionWriteRepository = sectionWriteRepository;
            _socialMediaLinkWriteRepository = socialMediaLinkWriteRepository;
        }

        public async Task<UpdateAboutCommandResponse> Handle(UpdateAboutCommandRequest request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var about = await _aboutReadRepository.Table
                .Include(a => a.Sections)
                    .ThenInclude(s => s.SectionImageFiles)
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

            var usedOrders = new HashSet<byte>();

            var existingOrders = about.Sections.Select(s => s.Order);
            usedOrders.UnionWith(existingOrders);

            if (request.Sections.Count == 0)
            {

                if (_sectionWriteRepository.DeleteRange(about.Sections))
                {
                    about.Sections.Clear();

                    await _storageService.DeleteAllAsync(AboutConst.ImagesFolder);
                }
            }
            else
            {
                if (about.Sections.Count > request.Sections.Count)
                {
                    var sectionsToDelete = about.Sections
                            .Where(s => !request.Sections.Select(r => r.Id).Contains(s.Id.ToString()))
                            .ToList();

                    if (_sectionWriteRepository.DeleteRange(sectionsToDelete))
                    {
                        foreach (var sectionToDelete in sectionsToDelete)
                        {
                            about.Sections.Remove(sectionToDelete);

                            foreach (var imageFile in sectionToDelete.SectionImageFiles)
                            {
                                await _storageService.DeleteAsync(imageFile.PathName, imageFile.FileName);
                            }
                        }
                    }
                }
                foreach (var sectionRequest in request.Sections)
                {
                    if (!string.IsNullOrWhiteSpace(sectionRequest.Id))
                    {
                        var section = about.Sections.FirstOrDefault(s => s.Id.ToString() == sectionRequest.Id);

                        if (section == null)
                        {
                            throw new EntityNotFoundException(nameof(section));
                        }

                        if (!string.IsNullOrWhiteSpace(sectionRequest.Title) && section.Title != sectionRequest.Title)
                        {
                            section.Title = sectionRequest.Title;
                        }

                        if (!string.IsNullOrWhiteSpace(sectionRequest.Description) && section.Description != sectionRequest.Description)
                        {
                            section.Description = sectionRequest.Description;
                        }

                        if (!string.IsNullOrWhiteSpace(sectionRequest.TextBody) && section.TextBody != sectionRequest.TextBody)
                        {
                            section.TextBody = sectionRequest.TextBody;
                        }

                        if (sectionRequest.Order >= 0 && sectionRequest.Order <= 255 && section.Order != sectionRequest.Order)
                        {
                            byte resolvedOrder = section.Order;
                            while (usedOrders.Contains(resolvedOrder))
                            {
                                resolvedOrder++;
                            }

                            usedOrders.Add(resolvedOrder);

                            section.Order = resolvedOrder;
                        }

                    }

                    else
                    {
                        var uploadResults = await _storageService.UploadMultipleAsync(AboutConst.ImagesFolder, sectionRequest.SectionImageFiles);

                        ICollection<AboutSectionImageFile> uploadedImages = [];

                        foreach (var (path, fileName) in uploadResults)
                        {
                            uploadedImages.Add(new AboutSectionImageFile
                            {
                                FileName = fileName,
                                PathName = path,
                                Storage = _storageService.StorageName,
                            });
                        }

                        byte resolvedOrder = sectionRequest.Order;
                        while (usedOrders.Contains(resolvedOrder))
                        {
                            resolvedOrder++;
                        }
                        usedOrders.Add(resolvedOrder);

                        // Create the new section with the resolved unique order
                        var newSection = new AboutSection
                        {
                            Title = sectionRequest.Title,
                            TextBody = sectionRequest.TextBody,
                            Description = sectionRequest.Description,
                            SectionImageFiles = uploadedImages,
                            Order = resolvedOrder
                        };

                        about.Sections.Add(newSection);
                    }
                }
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
