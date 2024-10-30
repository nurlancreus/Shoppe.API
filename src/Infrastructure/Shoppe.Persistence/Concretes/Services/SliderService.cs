using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.SlideRepos;
using Shoppe.Application.Abstractions.Repositories.SliderRepository;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Slide;
using Shoppe.Application.DTOs.Slider;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Sliders;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class SliderService : ISliderService
    {
        private readonly ISliderReadRepository _sliderReadRepository;
        private readonly ISliderWriteRepository _sliderWriteRepository;
        private readonly ISlideReadRepository _slideReadRepository;
        private readonly ISlideWriteRepository _slideWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSession _jwtSession;

        public SliderService(
            ISliderReadRepository sliderReadRepository,
            ISliderWriteRepository sliderWriteRepository,
            ISlideReadRepository slideReadRepository,
            ISlideWriteRepository slideWriteRepository,
            IStorageService storageService,
            IUnitOfWork unitOfWork,
            IJwtSession jwtSession)
        {
            _sliderReadRepository = sliderReadRepository;
            _sliderWriteRepository = sliderWriteRepository;
            _slideReadRepository = slideReadRepository;
            _slideWriteRepository = slideWriteRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
            _jwtSession = jwtSession;
        }

        public async Task CreateSliderAsync(CreateSliderDTO createSliderDTO, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            Slider slider = createSliderDTO.Type switch
            {
                SliderType.Hero => new HeroSlider(),
                _ => throw new AddNotSucceedException("Cannot add new slider. Invalid Slider type")
            };

            if (createSliderDTO.Slides.Count > 0)
            {
                var usedOrders = new HashSet<int>();

                foreach (var slide in createSliderDTO.Slides)
                {
                    var (path, fileName) = await _storageService.UploadAsync(SliderConst.ImagesFolder, slide.SlideImageFile);

                    byte resolvedOrder = slide.Order;
                    while (usedOrders.Contains(resolvedOrder))
                    {
                        resolvedOrder++;
                    }
                    usedOrders.Add(resolvedOrder);

                    slider.Slides.Add(new Slide
                    {
                        Body = slide.Body,
                        ButtonText = slide.ButtonText,
                        Title = slide.Title,
                        URL = slide.URL,
                        Order = resolvedOrder,
                        SlideImageFile = new SlideImageFile
                        {
                            FileName = fileName,
                            PathName = path,
                            Storage = _storageService.StorageName,
                        }
                    });
                }
            }

            await _sliderWriteRepository.AddAsync(slider, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }

        public async Task DeleteSlideAsync(string slideId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slide = await _slideReadRepository.Table.Include(s => s.Slider).ThenInclude(s => s.Slides)
                .Include(s => s.SlideImageFile)
                .FirstOrDefaultAsync(s => s.Id.ToString() == slideId, cancellationToken);

            if (slide == null)
            {
                throw new EntityNotFoundException(nameof(slide));
            }

            if (slide.Slider.Slides.Remove(slide))
            {
                _slideWriteRepository.Delete(slide);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // is it going to read file even after i save changes?
                await _storageService.DeleteAsync(slide.SlideImageFile.PathName, slide.SlideImageFile.FileName);
            }

            scope.Complete();
        }

        public async Task DeleteSliderAsync(string sliderId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slider = await _sliderReadRepository.Table.Include(s => s.Slides)
                .ThenInclude(s => s.SlideImageFile)
                .FirstOrDefaultAsync(s => s.Id.ToString() == sliderId, cancellationToken);

            if (slider == null)
            {
                throw new EntityNotFoundException(nameof(slider));
            }

            _sliderWriteRepository.Delete(slider);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var slide in slider.Slides)
            {
                await _storageService.DeleteAsync(slide.SlideImageFile.PathName, slide.SlideImageFile.FileName);
            }

            scope.Complete();
        }

        public async Task<List<GetSliderDTO>> GetAllSlidersAsync(CancellationToken cancellationToken)
        {
            var sliders = await _sliderReadRepository.Table.Include(s => s.Slides).ThenInclude(s => s.SlideImageFile).AsNoTracking().ToListAsync(cancellationToken);
            return sliders.Select(MapSliderToDTO).ToList();
        }

        public async Task<GetSliderDTO> GetSliderAsync(string sliderId, CancellationToken cancellationToken)
        {
            var slider = await _sliderReadRepository.Table.Include(s => s.Slides).ThenInclude(s => s.SlideImageFile).AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.ToString() == sliderId, cancellationToken);

            if (slider == null)
            {
                throw new EntityNotFoundException(nameof(slider));
            }

            return MapSliderToDTO(slider);
        }

        public async Task<GetSliderDTO> GetSliderByTypeAsync(SliderType sliderType, CancellationToken cancellationToken)
        {
            var slider = await _sliderReadRepository.Table.Include(s => s.Slides).ThenInclude(s => s.SlideImageFile).AsNoTracking()
                .FirstOrDefaultAsync(s => s.Type == sliderType.ToString(), cancellationToken);

            if (slider == null)
            {
                throw new EntityNotFoundException(nameof(slider));
            }

            return MapSliderToDTO(slider);
        }

        public async Task UpdateSlideAsync(UpdateSlideDTO updateSlideDTO, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slide = await _slideReadRepository.Table.Include(s => s.SlideImageFile)
                .FirstOrDefaultAsync(s => s.Id.ToString() == updateSlideDTO.SlideId, cancellationToken);

            if (slide == null)
            {
                throw new EntityNotFoundException(nameof(slide));
            }

            // Update slide properties
            if (updateSlideDTO.Body is string body && slide.Body != body)
            {
                slide.Body = body;
            }

            if (updateSlideDTO.ButtonText is string buttonText && slide.ButtonText != buttonText)
            {
                slide.ButtonText = buttonText;
            }

            if (updateSlideDTO.Title is string title && slide.Title != title)
            {
                slide.Title = title;
            }

            if (updateSlideDTO.URL is string url && slide.URL != url)
            {
                slide.URL = url;
            }

            if (updateSlideDTO.Order is byte order && slide.Order != order)
            {
                var usedOrders = new HashSet<byte>();

                var sliderOrders = _sliderReadRepository.Table.Include(s => s.Slides).SelectMany(s => s.Slides).Select(s => s.Order).AsEnumerable();

                usedOrders.UnionWith(sliderOrders);

                byte resolvedOrder = order;

                while (usedOrders.Contains(resolvedOrder))
                {
                    resolvedOrder++;
                }

                slide.Order = resolvedOrder;
            }

            // Handle image update if necessary
            if (updateSlideDTO.SlideImageFile != null)
            {
                var (path, fileName) = await _storageService.UploadAsync(SliderConst.ImagesFolder, updateSlideDTO.SlideImageFile);
                slide.SlideImageFile.FileName = fileName;
                slide.SlideImageFile.PathName = path;
                slide.SlideImageFile.Storage = _storageService.StorageName;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }

        public async Task UpdateSliderAsync(UpdateSliderDTO updateSliderDTO, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slider = await _sliderReadRepository.Table.Include(s => s.Slides).ThenInclude(s => s.SlideImageFile)
                .FirstOrDefaultAsync(s => s.Id.ToString() == updateSliderDTO.SliderId, cancellationToken);

            if (slider == null)
            {
                throw new EntityNotFoundException(nameof(slider));
            }

            if (_slideWriteRepository.DeleteRange(slider.Slides))
            {
                slider.Slides.Clear();
            }

            var usedOrders = new HashSet<byte>();

            foreach (var slide in updateSliderDTO.Slides)
            {
                var (path, fileName) = await _storageService.UploadAsync(SliderConst.ImagesFolder, slide.SlideImageFile);

                byte resolvedOrder = slide.Order;
                while (usedOrders.Contains(resolvedOrder))
                {
                    resolvedOrder++;
                }

                usedOrders.Add(resolvedOrder);

                slider.Slides.Add(new Slide
                {
                    Body = slide.Body,
                    ButtonText = slide.ButtonText,
                    Title = slide.Title,
                    URL = slide.URL,
                    Order = resolvedOrder,
                    SlideImageFile = new SlideImageFile
                    {
                        FileName = fileName,
                        PathName = path,
                        Storage = _storageService.StorageName,
                    }
                });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }

        private void ValidateAdminAccess()
        {
            if (!_jwtSession.IsAdmin())
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        private GetSliderDTO MapSliderToDTO(Slider slider)
        {
            return new GetSliderDTO
            {
                Id = slider.Id.ToString(),
                Slides = slider.Slides.Select(s => new GetSlideDTO
                {
                    Id = s.Id.ToString(),
                    Body = s.Body,
                    ButtonText = s.ButtonText,
                    ImageFile = new GetImageFileDTO
                    {
                        Id = s.SlideImageFile.Id.ToString(),
                        FileName = s.SlideImageFile.FileName,
                        PathName = s.SlideImageFile.PathName,
                        CreatedAt = s.CreatedAt,
                    },
                    CreatedAt = s.CreatedAt,
                }).ToList(),
                Type = slider.Type,
                CreatedAt = slider.CreatedAt,
            };
        }
    }
}
