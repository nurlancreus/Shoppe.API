using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.ProductDetailsRepos;
using Shoppe.Application.Abstractions.Repositories.ProductRepos;
using Shoppe.Application.Abstractions.Repositories.ReviewRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Product;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
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
        private readonly IPaginationService _paginationService;
        private readonly IProductDetailsReadRepository _productDetailsReadRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IUnitOfWork unitOfWork, IStorageService storageService, IPaginationService paginationService, IProductDetailsReadRepository productDetailsReadRepository, ICategoryReadRepository categoryReadRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _paginationService = paginationService;
            _productDetailsReadRepository = productDetailsReadRepository;
            _categoryReadRepository = categoryReadRepository;
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
                    Colors = createProductDTO.Colors.Select(Enum.Parse<Color>).ToList(),
                    Materials = createProductDTO.Materials.Select(Enum.Parse<Material>).ToList(),
                    Weigth = createProductDTO.Weigth,

                    Dimension = new ProductDimension()
                    {
                        Height = createProductDTO.Height,
                        Width = createProductDTO.Width,
                    }
                }
            };

            if (createProductDTO.CategoryIds.Count > 0)
            {
                foreach (var id in createProductDTO.CategoryIds)
                {
                    var category = await _categoryReadRepository.GetByIdAsync(id, cancellationToken);

                    if (category != null && category is ProductCategory productCategory)
                    {
                        product.Categories.Add(productCategory);

                    }
                }
            }

            if (createProductDTO.ProductImages.Count > 0)
            {
                List<(string path, string fileName)> imageResults = await _storageService.UploadAsync(ProductConst.ProductImagesFolder, createProductDTO.ProductImages);

                int counter = 0;

                foreach (var (path, fileName) in imageResults)
                {
                    product.ProductImageFiles.Add(new ProductImageFile()
                    {
                        FileName = fileName,
                        PathName = path,
                        Storage = _storageService.StorageName,
                        IsMain = ++counter == 1
                    });
                }
            }

            await _productWriteRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteProductAsync(string id, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            bool isDeleted = _productWriteRepository.Delete(product);

            if (!isDeleted)
            {
                throw new DeleteNotSucceedException(nameof(product));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetAllProductsDTO> GetAllProductsAsync(ProductFilterParamsDTO filtersDTO, CancellationToken cancellationToken)
        {
            var productsQuery = _productReadRepository.Table
                .Include(q => q.Categories)
                .Include(p => p.Reviews)
                .Include(q => q.ProductDetails)
                .ThenInclude(p => p.Dimension)
                .Include(q => q.ProductImageFiles)
                .AsNoTrackingWithIdentityResolution()
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtersDTO.CategoryName))
            {
                productsQuery = productsQuery.Where(p => p.Categories.Any(c => c.Name == filtersDTO.CategoryName));
            }

            if (filtersDTO.InStock == true)
            {
                productsQuery = productsQuery.Where(p => p.Stock > 0);
            }

            if (filtersDTO.MinPrice.HasValue && filtersDTO.MaxPrice.HasValue)
            {
                if (filtersDTO.MaxPrice < filtersDTO.MinPrice)
                {
                    throw new InvalidFilterException($"MaxPrice ({filtersDTO.MaxPrice}) cannot be less than MinPrice ({filtersDTO.MinPrice}).");
                }

                productsQuery = productsQuery.Where(p => p.Price >= filtersDTO.MinPrice.Value && p.Price <= filtersDTO.MaxPrice.Value);
            }

            if (filtersDTO.SortOptions.Count > 0)
            {
                foreach (var sortBy in filtersDTO.SortOptions)
                {
                    if (sortBy is SortOption sortOption)
                    {
                        productsQuery = sortOption switch
                        {
                            SortOption.PriceDesc => productsQuery.OrderByDescending(p => p.Price),
                            SortOption.PriceAsc => productsQuery.OrderBy(p => p.Price),
                            SortOption.CreatedAtDesc => productsQuery.OrderByDescending(p => p.CreatedAt),
                            SortOption.CreatedAtAsc => productsQuery.OrderBy(p => p.CreatedAt),
                            _ => productsQuery
                        };
                    }
                }
            }

            (int totalItems, int pageSize, int page, int totalPages, IQueryable<Product> paginatedQuery) = await _paginationService.ConfigurePaginationAsync(filtersDTO.Page, filtersDTO.PageSize, productsQuery);

            var products = await paginatedQuery.ToListAsync(cancellationToken);

            return new GetAllProductsDTO()
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Products = products.Select(p => new GetProductDTO()
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    Weigth = p.ProductDetails.Weigth,
                    Height = p.ProductDetails.Dimension.Height,
                    Width = p.ProductDetails.Dimension.Width,
                    Colors = p.ProductDetails.Colors.Select(c => c.ToString()).ToList(),
                    Materials = p.ProductDetails.Materials.Select(m => m.ToString()).ToList(),
                    Categories = p.Categories.Select(c => c.ToGetCategoryDTO()).ToList(),
                    ProductImages = p.ProductImageFiles.Select(i => new GetProductImageFileDTO()
                    {
                        FileName = i.FileName,
                        PathName = i.PathName,
                        IsMain = i.IsMain,
                    }).ToList(),

                    Rating = FindAverageRating(p.Reviews),
                    CreatedAt = p.CreatedAt
                }).ToList(),
            };
        }

        public async Task<GetProductDTO> GetProductAsync(string id, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(id, cancellationToken, false);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            await _productReadRepository.Table.Entry(product).Reference(p => p.ProductDetails).LoadAsync();

            await _productReadRepository.Table.Entry(product).Collection(p => p.ProductImageFiles).LoadAsync();

            await _productReadRepository.Table.Entry(product).Collection(p => p.Reviews).LoadAsync();

            await _productDetailsReadRepository.Table.Entry(product.ProductDetails).Reference(p => p.Dimension).LoadAsync();

            return new GetProductDTO()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Weigth = product.ProductDetails.Weigth,
                Height = product.ProductDetails.Dimension.Height,
                Width = product.ProductDetails.Dimension.Width,
                Colors = product.ProductDetails.Colors.Select(c => c.ToString()).ToList(),
                Materials = product.ProductDetails.Materials.Select(m => m.ToString()).ToList(),
                Categories = product.Categories.Select(c => c.ToGetCategoryDTO()).ToList(),
                ProductImages = product.ProductImageFiles.Select(i => new GetProductImageFileDTO()
                {
                    FileName = i.FileName,
                    PathName = i.PathName,
                    IsMain = i.IsMain,
                }).ToList(),

                Rating = FindAverageRating(product.Reviews),
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<List<GetReviewDTO>> GetReviewsByProductAsync(string productId, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(productId, cancellationToken, false);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            await _productReadRepository.Table.Entry(product).Collection(p => p.Reviews).LoadAsync(cancellationToken);

            return product.Reviews.Select(r => new GetReviewDTO()
            {
                Id = r.Id.ToString(),
                FirstName = r.FirstName!,
                LastName = r.LastName!,
                Rating = (int)r.Rating,
                Body = r.Body,
                CreatedAt = r.CreatedAt,
            }).ToList();
        }

        public async Task UpdateProductAsync(UpdateProductDTO updateProductDTO, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(updateProductDTO.Id, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException("product");
            }

            // Load related entities
            await _productReadRepository.Table.Entry(product).Reference(p => p.ProductDetails).LoadAsync(cancellationToken);
            await _productReadRepository.Table.Entry(product).Collection(p => p.ProductImageFiles).LoadAsync(cancellationToken);
            await _productDetailsReadRepository.Table.Entry(product.ProductDetails).Reference(p => p.Dimension).LoadAsync(cancellationToken);

            // Check and update properties only if they exist in the DTO
            if (!string.IsNullOrWhiteSpace(updateProductDTO.Name))
                product.Name = updateProductDTO.Name;

            if (updateProductDTO.Stock.HasValue)
                product.Stock = updateProductDTO.Stock.Value;

            if (updateProductDTO.Price.HasValue)
                product.Price = updateProductDTO.Price.Value;

            if (!string.IsNullOrWhiteSpace(updateProductDTO.Description))
                product.Description = updateProductDTO.Description;

            if (updateProductDTO.Weigth.HasValue)
                product.ProductDetails.Weigth = updateProductDTO.Weigth.Value;

            if (updateProductDTO.Width.HasValue && updateProductDTO.Height.HasValue)
            {
                product.ProductDetails.Dimension.Width = updateProductDTO.Width.Value;
                product.ProductDetails.Dimension.Height = updateProductDTO.Height.Value;
            }

            // Update material if the material is provided
            if (updateProductDTO.Materials.Count > 0)
            {
                product.ProductDetails.Materials.Clear(); // Clear existing colors
                foreach (var material in updateProductDTO.Materials)
                {
                    if (EnumHelpers.TryParseEnum(material, out Material parsedMaterial))
                    {
                        product.ProductDetails.Materials.Add(parsedMaterial);
                    }
                }
            }

            // Update colors if any colors are provided
            if (updateProductDTO.Colors.Count > 0)
            {
                product.ProductDetails.Colors.Clear(); // Clear existing colors
                foreach (var color in updateProductDTO.Colors)
                {
                    if (EnumHelpers.TryParseEnum(color, out Color parsedColor))
                    {
                        product.ProductDetails.Colors.Add(parsedColor);
                    }
                }
            }

            if (updateProductDTO.CategoryIds.Count > 0)
            {

                foreach (var id in updateProductDTO.CategoryIds)
                {
                    var category = await _categoryReadRepository.GetByIdAsync(id, cancellationToken);

                    if (category is ProductCategory productCategory )
                    {
                        product.Categories.Add(productCategory);

                    }
                }
            }

            // Handle product images if any are provided
            if (updateProductDTO.ProductImages.Count > 0)
            {
                List<(string path, string fileName)> imageResults = await _storageService.UploadAsync(ProductConst.ProductImagesFolder, updateProductDTO.ProductImages);

                var existingMainImage = product.ProductImageFiles.SingleOrDefault(i => i.IsMain);
                int counter = 0;

                foreach (var (path, fileName) in imageResults)
                {
                    var productImage = new ProductImageFile()
                    {
                        FileName = fileName,
                        PathName = path,
                        Storage = _storageService.StorageName,
                    };

                    if (existingMainImage == null && ++counter == 1)
                    {
                        productImage.IsMain = true;
                    }

                    product.ProductImageFiles.Add(productImage);
                }
            }

            // Update the product in the repository
            bool isUpdated = _productWriteRepository.Update(product);

            if (!isUpdated)
            {
                throw new UpdateNotSucceedException(nameof(product));
            }


            // Commit changes to the database
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }


        private static float FindAverageRating(ICollection<Review> reviews)
        {
            var ratingSum = reviews.Sum(r => (int)r.Rating);

            var reviewCount = reviews.Count;

            var rating = reviewCount > 0 ? (float)ratingSum / reviewCount : 0;

            return (float)Math.Round(rating, 2);
        }
    }
}
