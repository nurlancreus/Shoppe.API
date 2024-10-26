using Shoppe.Application.DTOs.Slide;
using Shoppe.Application.DTOs.Slider;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services
{
    public interface ISliderService
    {
        Task CreateSliderAsync(CreateSliderDTO createSliderDTO, CancellationToken cancellationToken);
        Task DeleteSliderAsync(string sliderId, CancellationToken cancellationToken);
        Task UpdateSliderAsync(UpdateSliderDTO updateSliderDTO, CancellationToken cancellationToken);
        Task<GetSliderDTO> GetSliderAsync(string sliderId, CancellationToken cancellationToken);
        Task<GetSliderDTO> GetSliderByTypeAsync(SliderType sliderType, CancellationToken cancellationToken);
        Task DeleteSlideAsync(string slideId, CancellationToken cancellationToken);
        Task UpdateSlideAsync(UpdateSlideDTO updateSlideDTO, CancellationToken cancellationToken);
        Task<List<GetSliderDTO>> GetAllSlidersAsync(CancellationToken cancellationToken);
    }
}
