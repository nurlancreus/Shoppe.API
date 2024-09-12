using MediatR;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Product.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductAsync(request.Id!, cancellationToken);

            return new DeleteProductCommandResponse()
            {
                IsSuccess = true,
                Message = ResponseConst.DeletedSuccessMessage("product")
            };
        }
    }
}
