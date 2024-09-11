using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Product;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;

        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IUnitOfWork unitOfWork, IStorageService storageService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public async Task CreateProductAsync(CreateProductDTO createProductDTO, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = createProductDTO.Name,
                Description = createProductDTO.Description,
                Price = createProductDTO.Price,
                Stock = createProductDTO.Stock,
                ProductDetails = new ProductDetails()
                {
                    Colors = createProductDTO.Colors.Select(color => Enum.Parse<Color>(color)).ToList(),
                    Material = Enum.Parse<Material>(createProductDTO.Material),
                    Weigth = createProductDTO.Weigth,

                    Dimension = new ProductDimension()
                    {
                        Height = createProductDTO.Height,
                        Width = createProductDTO.Width,
                    }
                }
            };

            if(createProductDTO.Categories.Count > 0) 
            { 
                product.Categories = createProductDTO.Categories.Select(c => c.ToCategoryEntity()).ToList();
            }

            if (createProductDTO.ProductImages.Count > 0)
            {
                List<(string path, string fileName)> imageResults = await _storageService.UploadAsync(ProductConst.ProductImagesFolder, createProductDTO.ProductImages);

                int counter = 0;

                foreach (var imageResult in imageResults)
                {
                    product.ProductImageFiles.Add(new ProductImageFile()
                    {
                        FileName = imageResult.fileName,
                        PathName = imageResult.path,
                        Storage = _storageService.StorageName,
                        IsMain = ++counter == 1
                    });
                }

                await _productWriteRepository.AddAsync(product, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
