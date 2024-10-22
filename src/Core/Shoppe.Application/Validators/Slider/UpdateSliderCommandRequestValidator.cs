using FluentValidation;
using Shoppe.Application.Features.Command.Slider.Update;
using Shoppe.Application.DTOs.Slide;

public class UpdateSliderCommandRequestValidator : AbstractValidator<UpdateSliderCommandRequest>
{
    public UpdateSliderCommandRequestValidator()
    {
        RuleFor(x => x.SliderId)
            .NotEmpty().WithMessage("SliderId is required.");

        RuleFor(x => x.Slides)
            .ForEach(slide => slide.SetValidator(new CreateSlideDTOValidator()))
            .When(x => x.Slides != null && x.Slides.Count != 0);
    }
}
