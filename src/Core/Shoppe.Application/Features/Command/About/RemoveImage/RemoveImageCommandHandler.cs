using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly ISectionReadRepository _sectionReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveImageCommandHandler(ISectionReadRepository sectionReadRepository, IFileWriteRepository fileWriteRepository, IStorageService storageService, IUnitOfWork unitOfWork)
        {
            _sectionReadRepository = sectionReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<RemoveImageCommandResponse> Handle(RemoveImageCommandRequest request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var section = await _sectionReadRepository.Table.OfType<AboutSection>().Include(s => s.SectionImageFiles).FirstOrDefaultAsync(s => s.Id.ToString() == request.SectionId, cancellationToken);


            if (section == null)
            {
                throw new EntityNotFoundException(nameof(section));
            }

            var image = section.SectionImageFiles.FirstOrDefault(i => i.Id.ToString() == request.ImageId);

            if (image == null)
            {
                throw new EntityNotFoundException(nameof(image));

            }

            bool isRemoved = section.SectionImageFiles.Remove(image);


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
