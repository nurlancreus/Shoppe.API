using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Product.RemoveImage
{
    public class RemoveImageCommandHandler : IRequestHandler<RemoveImageCommandRequest, RemoveImageCommandResponse>
    {
        private readonly IProductService _productService;

        public RemoveImageCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<RemoveImageCommandResponse> Handle(RemoveImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.RemoveImageAsync(request.ProductId!, request.ImageId!, cancellationToken);

            return new RemoveImageCommandResponse
            {
                IsSuccess = true
            };
        }
    }
}
