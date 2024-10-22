using FluentValidation;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Shoppe.Domain.Entities;
using System.Text.RegularExpressions;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.SocialMediaLink;
using Shoppe.Application.Features.Command.About.Update;
using Shoppe.Domain.Enums;
using Shoppe.Application.Helpers;
using Shoppe.Application.Constants;
using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Microsoft.EntityFrameworkCore;

public class UpdateAboutCommandValidator : AbstractValidator<UpdateAboutCommandRequest>
{
    private readonly IAboutReadRepository _aboutReadRepository;
    public UpdateAboutCommandValidator(IAboutReadRepository aboutReadRepository)
    {
        _aboutReadRepository = aboutReadRepository;

        // Optional Name
        RuleFor(x => x.Name)
            .MaximumLength(AboutConst.MaxNameLength).WithMessage($"Name cannot be longer than {AboutConst.MaxNameLength} characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        // Optional Title
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        // Optional Description
        RuleFor(x => x.Description)
            .MaximumLength(AboutConst.MaxDescLength).WithMessage($"Description cannot be longer than {AboutConst.MaxDescLength} characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        // Validate Email if provided
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        // Validate Phone if provided
        RuleFor(x => x.Phone)
            .Matches(@"^\+?\d{1,3}?[-.●]?\(?\d{1,4}?\)?[-.●]?\d{1,4}[-.●]?\d{1,9}$")
            .WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        // Validate Sections
        RuleForEach(x => x.Sections)
            .SetValidator(new CreateSectionDTOValidator())
            .When(x => x.Sections != null && x.Sections.Count > 0);

        // Validate SocialMediaLinks
        RuleForEach(x => x.SocialMediaLinks)
            .SetValidator(new CreateSocialMediaLinkDTOValidator(_aboutReadRepository))
            .When(x => x.SocialMediaLinks != null && x.SocialMediaLinks.Count > 0);

        // Check for duplicate social media platforms
        RuleFor(x => x.SocialMediaLinks)
            .Must(links => links.Select(l => l.SocialPlatform).Distinct(StringComparer.OrdinalIgnoreCase).Count() == links.Count)
            .WithMessage("Duplicate social media platforms are not allowed.")
            .When(x => x.SocialMediaLinks != null && x.SocialMediaLinks.Count > 0);
    }
}

public class CreateSectionDTOValidator : AbstractValidator<CreateSectionDTO>
{
    public CreateSectionDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Section title cannot be empty.")
            .MaximumLength(100).WithMessage("Section title cannot be longer than 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Section description cannot be empty.")
            .MaximumLength(500).WithMessage("Section description cannot be longer than 500 characters.");

        RuleForEach(x => x.SectionImageFiles)
            .Must(file => file.IsImage()).WithMessage("Only image files are allowed.")
            .Must(file => file.IsSizeOk(AboutConst.MaxFileSizeInMb)).WithMessage($"Image size cannot exceed {AboutConst.MaxFileSizeInMb}MB.")
            .Must(file => file.RestrictExtension([".jpg", ".png"])).WithMessage("Allowed file extensions are .jpg, .png.")
            .Must(file => file.RestrictMimeTypes(["image/jpeg", "image/png"])).WithMessage("Allowed mime types are image/jpeg, image/png.");
    }
}

public class CreateSocialMediaLinkDTOValidator : AbstractValidator<CreateSocialMediaLinkDTO>
{
    private readonly IAboutReadRepository _aboutReadRepository;
    public CreateSocialMediaLinkDTOValidator(IAboutReadRepository aboutReadRepository)
    {
        _aboutReadRepository = aboutReadRepository;

        RuleFor(x => x.URL)
            .NotEmpty().WithMessage("URL cannot be empty.")
            .Must(BeAValidUrl).WithMessage("Invalid URL format.");

        RuleFor(x => x.SocialPlatform)
            .NotEmpty().WithMessage("Social platform cannot be empty.")
            .Must(BeAValidPlatform).WithMessage("Invalid social media platform.")
            .MustAsync(CheckIfPlatformAlreadyDefinedAsync)
            .WithMessage("You already defined the same platform.");
    }

    private async Task<bool> CheckIfPlatformAlreadyDefinedAsync(string platform, CancellationToken cancellationToken)
    {
        var about = await _aboutReadRepository.Table.Include(a => a.SocialMediaLinks).SingleOrDefaultAsync(cancellationToken);

        if (about == null)
        {
            return false;
        }

        if (about.SocialMediaLinks.Any(sm => sm.SocialPlatform.ToString().ToLower() == platform.ToLower())) return false;

        return true;
    }

    private bool BeAValidUrl(string url)
    {
        // Basic URL validation (you can improve with more complex logic if needed)
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private bool BeAValidPlatform(string platform)
    {
        return Enum.TryParse(typeof(SocialPlatform), platform, true, out _); // Case-insensitive check
    }
}
