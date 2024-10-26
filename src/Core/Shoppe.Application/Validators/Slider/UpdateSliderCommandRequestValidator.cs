using FluentValidation;
using Shoppe.Application.Features.Command.Slider.Update;
using Shoppe.Application.DTOs.Slide;

public class UpdateSliderCommandRequestValidator : AbstractValidator<UpdateSliderCommandRequest>
{
    public UpdateSliderCommandRequestValidator()
    {
        RuleFor(x => x.SliderId)
            .NotEmpty().WithMessage("SliderId is required.");

        RuleFor(x => x.NewSlides)
            .ForEach(slide => slide.SetValidator(new CreateSlideDTOValidator()))
            .When(x => x.NewSlides != null && x.NewSlides.Count != 0);

        RuleFor(x => x.UpdatedSlides)
            .ForEach(slide => slide.SetValidator(new UpdateSlideDTOValidator()))
            .When(x => x.UpdatedSlides != null && x.UpdatedSlides.Count != 0);
    }
}
