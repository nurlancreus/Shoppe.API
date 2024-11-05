using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.FileRepos;
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
using System.Drawing;
using System.IO;

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
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IJwtSession _jwtSession;

        public SliderService(
            ISliderReadRepository sliderReadRepository,
            ISliderWriteRepository sliderWriteRepository,
            ISlideReadRepository slideReadRepository,
            ISlideWriteRepository slideWriteRepository,
            IStorageService storageService,
            IUnitOfWork unitOfWork,
            IJwtSession jwtSession,
            IFileWriteRepository fileWriteRepository)
        {
            _sliderReadRepository = sliderReadRepository;
            _sliderWriteRepository = sliderWriteRepository;
            _slideReadRepository = slideReadRepository;
            _slideWriteRepository = slideWriteRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
            _jwtSession = jwtSession;
            _fileWriteRepository = fileWriteRepository;
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
                var usedOrders = new HashSet<byte>();

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

        public async Task DeleteSlideAsync(Guid slideId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slide = await _slideReadRepository.Table.Include(s => s.Slider).ThenInclude(s => s.Slides)
                .Include(s => s.SlideImageFile)
                .FirstOrDefaultAsync(s => s.Id == slideId, cancellationToken);

            if (slide == null)
            {
                throw new EntityNotFoundException(nameof(slide));
            }

            if (slide.Slider.Slides.Remove(slide))
            {
                if (_slideWriteRepository.Delete(slide) && await _unitOfWork.SaveChangesAsync(cancellationToken))
                {

                    await _storageService.DeleteAsync(slide.SlideImageFile.PathName, slide.SlideImageFile.FileName);
                    scope.Complete();
                }
            }
        }

        public async Task DeleteSliderAsync(Guid sliderId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slider = await _sliderReadRepository.Table.Include(s => s.Slides)
                .ThenInclude(s => s.SlideImageFile)
                .FirstOrDefaultAsync(s => s.Id == sliderId, cancellationToken);

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

        public async Task<GetSliderDTO> GetSliderAsync(Guid sliderId, CancellationToken cancellationToken)
        {
            var slider = await _sliderReadRepository.Table.Include(s => s.Slides).ThenInclude(s => s.SlideImageFile).AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sliderId, cancellationToken);

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
                .FirstOrDefaultAsync(s => s.Id == updateSlideDTO.SlideId, cancellationToken);

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            scope.Complete();
        }

        public async Task UpdateSliderAsync(UpdateSliderDTO updateSliderDTO, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slider = await _sliderReadRepository.Table.Include(s => s.Slides).ThenInclude(s => s.SlideImageFile)
                .FirstOrDefaultAsync(s => s.Id == updateSliderDTO.SliderId, cancellationToken);

            if (slider == null)
            {
                throw new EntityNotFoundException(nameof(slider));
            }

            var usedOrders = new HashSet<byte>();

            var existingOrders = slider.Slides.Select(s => s.Order);
            usedOrders.UnionWith(existingOrders);

            if (updateSliderDTO.UpdatedSlides.Count == 0 && slider.Slides.Count != 0)
            {
                if (_slideWriteRepository.DeleteRange(slider.Slides))
                {
                    var slidesImages = slider.Slides.Select(s => s.SlideImageFile);
                    slider.Slides.Clear();

                    foreach (var slideImageFile in slidesImages)
                    {
                        await _storageService.DeleteAsync(slideImageFile.PathName, slideImageFile.FileName);
                    }
                }

            }
            else
            {
                if (slider.Slides.Count > updateSliderDTO.UpdatedSlides.Count)
                {
                    var slidesToDelete = slider.Slides
                            .Where(s => !updateSliderDTO.UpdatedSlides.Select(r => r.SlideId).Contains(s.Id))
                            .ToList();

                    if (_slideWriteRepository.DeleteRange(slidesToDelete))
                    {
                        foreach (var slideToDelete in slidesToDelete)
                        {
                            slider.Slides.Remove(slideToDelete);

                            await _storageService.DeleteAsync(slideToDelete.SlideImageFile.PathName, slideToDelete.SlideImageFile.FileName);

                        }
                    }
                }
                foreach (var slideRequest in updateSliderDTO.UpdatedSlides)
                {
                    if (slideRequest.SlideId is Guid slideId)
                    {
                        var slide = slider.Slides.FirstOrDefault(s => s.Id == slideId);

                        if (slide == null)
                        {
                            throw new EntityNotFoundException(nameof(slide));
                        }

                        if (!string.IsNullOrWhiteSpace(slideRequest.Title) && slide.Title != slideRequest.Title)
                        {
                            slide.Title = slideRequest.Title;
                        }

                        if (!string.IsNullOrWhiteSpace(slideRequest.Body) && slide.Body != slideRequest.Body)
                        {
                            slide.Body = slideRequest.Body;
                        }

                        if (!string.IsNullOrWhiteSpace(slideRequest.ButtonText) && slide.ButtonText != slideRequest.ButtonText)
                        {
                            slide.ButtonText = slideRequest.ButtonText;
                        }

                        if (!string.IsNullOrWhiteSpace(slideRequest.URL) && slide.URL != slideRequest.URL)
                        {
                            slide.URL = slideRequest.URL;
                        }

                        if (slideRequest.Order is byte order && order >= 0 && order <= 255 && slide.Order != order)
                        {
                            byte resolvedOrder = order;
                            while (usedOrders.Contains(resolvedOrder))
                            {
                                resolvedOrder++;
                            }

                            usedOrders.Add(resolvedOrder);

                            slide.Order = resolvedOrder;
                        }

                    }

                }
            }
            foreach (var slideRequest in updateSliderDTO.NewSlides)
            {

                var (path, fileName) = await _storageService.UploadAsync(SliderConst.ImagesFolder, slideRequest.SlideImageFile);

                byte resolvedOrder = slideRequest.Order;
                while (usedOrders.Contains(resolvedOrder))
                {
                    resolvedOrder++;
                }
                usedOrders.Add(resolvedOrder);

                slider.Slides.Add(new Slide
                {
                    Body = slideRequest.Body,
                    ButtonText = slideRequest.ButtonText,
                    Title = slideRequest.Title,
                    URL = slideRequest.URL,
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

        public async Task ChangeSlideImageAsync(Guid slideId, IFormFile newImageFile, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var slide = await _slideReadRepository.Table.Include(s => s.SlideImageFile).FirstOrDefaultAsync(s => s.Id == slideId, cancellationToken);


            if (slide == null)
            {
                throw new EntityNotFoundException(nameof(slide));
            }

            var image = slide.SlideImageFile;

            if (image == null)
            {
                throw new EntityNotFoundException(nameof(image));

            }

            if (newImageFile.Length > 0)
            {
                var (path, fileName) = await _storageService.UploadAsync(SliderConst.ImagesFolder, newImageFile);

                var newImage = new SlideImageFile
                {
                    FileName = fileName,
                    PathName = path,
                    Storage = _storageService.StorageName,
                };

                slide.SlideImageFile = newImage;
            }

            if (_fileWriteRepository.Delete(image))
            {
                if (await _unitOfWork.SaveChangesAsync(cancellationToken))
                {

                    await _storageService.DeleteAsync(image.PathName, image.FileName);
                }

                scope.Complete();
            }

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
                Id = slider.Id,
                Slides = slider.Slides.Select(s => new GetSlideDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    URL = s.URL,
                    Body = s.Body,
                    ButtonText = s.ButtonText,
                    Order = s.Order,
                    ImageFile = new GetImageFileDTO
                    {
                        Id = s.SlideImageFile.Id,
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
