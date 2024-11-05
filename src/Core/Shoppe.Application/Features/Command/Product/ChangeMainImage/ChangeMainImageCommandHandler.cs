using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Command.Product.ChangeMainImage
{
    public class ChangeMainImageCommandHandler : IRequestHandler<ChangeMainImageCommandRequest, ChangeMainImageCommandResponse>
    {
        private readonly IProductService _productService;

        public ChangeMainImageCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ChangeMainImageCommandResponse> Handle(ChangeMainImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.ChangeMainImageAsync((Guid)request.ProductId!, (Guid)request.ImageId!, cancellationToken);

            return new ChangeMainImageCommandResponse
            {
                IsSuccess = true,
            };
        }
    }
}
