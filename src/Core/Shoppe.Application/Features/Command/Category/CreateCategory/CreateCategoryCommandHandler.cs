using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Category.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
    {
        private readonly ICategoryService _categoryService;

        public CreateCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.CreateCategoryAsync(request.Name, cancellationToken);

            return new CreateCategoryCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.AddedSuccessMessage("Category")
            };
        }
    }
}
