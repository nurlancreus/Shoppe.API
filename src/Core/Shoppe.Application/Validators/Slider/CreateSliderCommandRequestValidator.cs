using FluentValidation;
using Shoppe.Application.Features.Command.Slider.Create;
using Shoppe.Application.DTOs.Slide;
using Shoppe.Application.Constants;
using Shoppe.Domain.Enums;

public class CreateSliderCommandRequestValidator : AbstractValidator<CreateSliderCommandRequest>
{
    public CreateSliderCommandRequestValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Slider type is required.")
            .Must(type => Enum.TryParse(typeof(SliderType), type, true, out _))
            .WithMessage("Invalid slider type.");

        RuleFor(x => x.NewSlides)
            .NotEmpty().WithMessage("Slides are required.")
            .ForEach(slide => slide.SetValidator(new CreateSlideDTOValidator()))
            .When(x => x.NewSlides != null && x.NewSlides.Count != 0);
    }
}
