using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.AboutRepos;
using Shoppe.Application.Abstractions.Repositories.FileRepos;
using Shoppe.Application.Abstractions.Repositories.SectionRepos;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Domain.Entities.Sections;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Application.Features.Command.About.RemoveImage
{
    public class RemoveImageCommandHandler : IRequestHandler<RemoveImageCommandRequest, RemoveImageCommandResponse>
    {
        private readonly IAboutReadRepository _aboutReadRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveImageCommandHandler(IFileWriteRepository fileWriteRepository, IStorageService storageService, IUnitOfWork unitOfWork, IFileReadRepository fileReadRepository, IAboutReadRepository aboutReadRepository)
        {
            _aboutReadRepository = aboutReadRepository;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<RemoveImageCommandResponse> Handle(RemoveImageCommandRequest request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var about = await _aboutReadRepository.Table.Include(a => a.ContentImages).FirstOrDefaultAsync(cancellationToken);

            if (about == null)
            {
                throw new EntityNotFoundException(nameof(about));

            }

            var image = about.ContentImages.FirstOrDefault(i => i.Id == request.ImageId);

            if (image == null)
            {
                throw new EntityNotFoundException(nameof(image));

            }

            bool isRemoved = about.ContentImages.Remove(image);

            if (isRemoved && _fileWriteRepository.Delete(image))
            {
                if (await _unitOfWork.SaveChangesAsync(cancellationToken))
                {

                    await _storageService.DeleteAsync(image.PathName, image.FileName);
                }

                scope.Complete();
            }

            return new RemoveImageCommandResponse
            {
                IsSuccess = true,
            };


        }
    }
}
