﻿using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
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
using Shoppe.Application.Extensions.Helpers;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Reviews;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewService _reviewService;
        private readonly IStorageService _storageService;
        private readonly IPaginationService _paginationService;
        private readonly IDiscountService _discountService;
        private readonly IProductDetailsReadRepository _productDetailsReadRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IDiscountReadRepository _discountReadRepository;
        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IUnitOfWork unitOfWork, IStorageService storageService, IPaginationService paginationService, IProductDetailsReadRepository productDetailsReadRepository, ICategoryReadRepository categoryReadRepository, IReviewService reviewService, IDiscountReadRepository discountReadRepository, IDiscountService discountService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _paginationService = paginationService;
            _productDetailsReadRepository = productDetailsReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _reviewService = reviewService;
            _discountReadRepository = discountReadRepository;
            _discountService = discountService;
        }

        public async Task CreateProductAsync(CreateProductDTO createProductDTO, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var product = new Product()
                {
                    Name = createProductDTO.Name,
                    Info = createProductDTO.Info,
                    Description = createProductDTO.Description,
                    Price = createProductDTO.Price,
                    Stock = createProductDTO.Stock,
                    ProductDetails = new ProductDetails()
                    {
                        Colors = createProductDTO.Colors.Select(Enum.Parse<Color>).ToList(),
                        Materials = createProductDTO.Materials.Select(Enum.Parse<Material>).ToList(),
                        Weight = createProductDTO.Weight,

                        Dimension = new ProductDimension()
                        {
                            Height = createProductDTO.Height,
                            Width = createProductDTO.Width,
                        }
                    }
                };

                if(!string.IsNullOrEmpty(createProductDTO.DiscountId))
                {
                    await _discountService.AssignDiscountAsync(product, createProductDTO.DiscountId, cancellationToken);
                }

                if (createProductDTO.Categories.Count > 0)
                {
                    foreach (var categoryName in createProductDTO.Categories)
                    {
                        var category = await _categoryReadRepository.GetAsync(c => c.Name == categoryName, cancellationToken);

                        if (category != null && category is ProductCategory productCategory)
                        {
                            product.Categories.Add(productCategory);
                        }
                    }
                }

                if (createProductDTO.ProductImages.Count > 0)
                {
                    List<(string path, string fileName)> imageResults = await _storageService.UploadMultipleAsync(ProductConst.ImagesFolder, createProductDTO.ProductImages);

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

                transaction.Complete(); // Commit the transaction if everything is successful
            }
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

            (int totalItems, int pageSize, int page, int totalPages, IQueryable<Product> paginatedQuery) = await _paginationService.ConfigurePaginationAsync(filtersDTO.Page, filtersDTO.PageSize, productsQuery, cancellationToken);

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
                    Info = p.Info,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    Weight = p.ProductDetails.Weight,
                    Height = p.ProductDetails.Dimension.Height,
                    Width = p.ProductDetails.Dimension.Width,
                    Colors = p.ProductDetails.Colors.Select(c => c.ToString()).ToList(),
                    Materials = p.ProductDetails.Materials.Select(m => m.ToString()).ToList(),
                    Categories = p.Categories.Select(c => c.ToGetCategoryDTO()).ToList(),
                    ProductImages = p.ProductImageFiles.Select(i => new GetProductImageFileDTO()
                    {
                        Id = i.Id.ToString(),
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
            await _productReadRepository.Table.Entry(product).Collection(p => p.Categories).LoadAsync();

            await _productDetailsReadRepository.Table.Entry(product.ProductDetails).Reference(p => p.Dimension).LoadAsync();

            return new GetProductDTO()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Info = product.Info,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Weight = product.ProductDetails.Weight,
                Height = product.ProductDetails.Dimension.Height,
                Width = product.ProductDetails.Dimension.Width,
                Colors = product.ProductDetails.Colors.Select(c => c.ToString()).ToList(),
                Materials = product.ProductDetails.Materials.Select(m => m.ToString()).ToList(),
                Categories = product.Categories.Select(c => c.ToGetCategoryDTO()).ToList(),
                ProductImages = product.ProductImageFiles.Select(i => new GetProductImageFileDTO()
                {
                    Id = i.Id.ToString(),
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
            var product = await _productReadRepository.Table.Include(p => p.Reviews).ThenInclude(r => r.Reviewer).FirstOrDefaultAsync(p => p.Id.ToString() == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            //await _productReadRepository.Table.Entry(product).Collection(p => p.Reviews).LoadAsync(cancellationToken);

            return product.Reviews.Select(r => new GetReviewDTO()
            {
                Id = r.Id.ToString(),
                FirstName = r.Reviewer?.FirstName!,
                LastName = r.Reviewer?.LastName!,
                Rating = (int)r.Rating,
                Body = r.Body,
                CreatedAt = r.CreatedAt,
            }).ToList();
        }

        public async Task UpdateProductAsync(UpdateProductDTO updateProductDTO, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var product = await _productReadRepository.Table.Include(p => p.ProductDetails).ThenInclude(pd => pd.Dimension).Include(p => p.Categories).Include(p => p.DiscountMappings).FirstOrDefaultAsync(p => p.Id.ToString() == updateProductDTO.Id, cancellationToken);

                if (product == null)
                {
                    throw new EntityNotFoundException(nameof(product));
                }

                // Load related entities
                //await _productReadRepository.Table.Entry(product).Reference(p => p.ProductDetails).LoadAsync(cancellationToken);
                //await _productReadRepository.Table.Entry(product).Collection(p => p.ProductImageFiles).LoadAsync(cancellationToken);
                //await _productReadRepository.Table.Entry(product).Collection(p => p.Categories).LoadAsync(cancellationToken);
                //await _productDetailsReadRepository.Table.Entry(product.ProductDetails).Reference(p => p.Dimension).LoadAsync(cancellationToken);

                // Update properties
                if (!string.IsNullOrWhiteSpace(updateProductDTO.Name))
                    product.Name = updateProductDTO.Name;

                if (updateProductDTO.Stock.HasValue)
                    product.Stock = updateProductDTO.Stock.Value;

                if (updateProductDTO.Price.HasValue)
                    product.Price = updateProductDTO.Price.Value;

                if (!string.IsNullOrWhiteSpace(updateProductDTO.Info))
                    product.Info = updateProductDTO.Info;

                if (!string.IsNullOrWhiteSpace(updateProductDTO.Description))
                    product.Description = updateProductDTO.Description;

                if (updateProductDTO.Weight.HasValue)
                    product.ProductDetails.Weight = updateProductDTO.Weight.Value;

                if (updateProductDTO.Width.HasValue && updateProductDTO.Height.HasValue)
                {
                    product.ProductDetails.Dimension.Width = updateProductDTO.Width.Value;
                    product.ProductDetails.Dimension.Height = updateProductDTO.Height.Value;
                }

                // Update materials
                if (updateProductDTO.Materials.Count > 0)
                {
                    product.ProductDetails.Materials.Clear(); // Clear existing materials
                    foreach (var material in updateProductDTO.Materials)
                    {
                        if (EnumHelpers.TryParseEnum(material, out Material parsedMaterial))
                        {
                            product.ProductDetails.Materials.Add(parsedMaterial);
                        }
                    }
                }

                // Update colors
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

                // Update categories
                if (updateProductDTO.Categories.Count > 0)
                {
                    // Remove categories not in the updated list
                    var categoriesToRemove = product.Categories
                        .Where(existingCategory => !updateProductDTO.Categories.Contains(existingCategory.Name))
                        .ToList();

                    foreach (var category in categoriesToRemove)
                    {
                        product.Categories.Remove(category);
                    }

                    // Add new categories
                    foreach (var name in updateProductDTO.Categories)
                    {
                        var category = await _categoryReadRepository.GetAsync(c => c.Name == name, cancellationToken);
                        if (category is ProductCategory productCategory && !product.Categories.Contains(category))
                        {
                            product.Categories.Add(productCategory);
                        }
                    }
                }

                if(!string.IsNullOrEmpty(updateProductDTO.DiscountId))
                {
                    await _discountService.AssignDiscountAsync(product, updateProductDTO.DiscountId, cancellationToken, update: true);
                }

                // Handle product images
                if (updateProductDTO.ProductImages.Count > 0)
                {
                    List<(string path, string fileName)> imageResults = await _storageService.UploadMultipleAsync(ProductConst.ImagesFolder, updateProductDTO.ProductImages);

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

                // Complete the transaction
                transaction.Complete();
            }
        }

        public async Task ChangeMainImageAsync(string productId, string newMainImageId, CancellationToken cancellationToken)
        {

            var product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id.ToString() == productId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            if (product.ProductImageFiles.Count == 0)
            {
                throw new EntityNotFoundException("product currently has no image.");
            }

            var existingMainImage = product.ProductImageFiles.FirstOrDefault(i => i.IsMain);

            var newMainImage = product.ProductImageFiles.FirstOrDefault(i => i.Id.ToString().ToLower() == newMainImageId.ToLower());

            if (newMainImage == null)
            {
                throw new EntityNotFoundException(nameof(newMainImage));
            }

            if (existingMainImage == newMainImage) return;

            if (existingMainImage != null)
            {
                existingMainImage.IsMain = false;

                // temporary solution!
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            newMainImage.IsMain = true;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task RemoveImageAsync(string productId, string imageId, CancellationToken cancellationToken)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var product = await _productReadRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            await _productReadRepository.Table.Entry(product).Collection(p => p.ProductImageFiles).LoadAsync();

            var imageToDelete = product.ProductImageFiles.FirstOrDefault(i => i.Id.ToString() == imageId);

            if (imageToDelete == null)
            {
                throw new EntityNotFoundException("Image not found to be deleted");
            }

            if (imageToDelete.IsMain)
            {
                throw new InvalidOperationException("You cannot remove the main image. Please change the main image first.");
            }

            bool isRemoved = product.ProductImageFiles.Remove(imageToDelete);

            if (isRemoved)
            {
                await _storageService.DeleteAsync(imageToDelete.PathName, imageToDelete.FileName);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }
        }

        private static float FindAverageRating(ICollection<ProductReview> reviews)
        {
            var ratingSum = reviews.Sum(r => (int)r.Rating);

            var reviewCount = reviews.Count;

            var rating = reviewCount > 0 ? (float)ratingSum / reviewCount : 0;

            return (float)Math.Round(rating, 2);
        }


    }
}
