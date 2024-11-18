using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.FileRepos;
using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Content;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Blog;
using Shoppe.Application.DTOs.Category;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.DTOs.Tag;
using Shoppe.Application.DTOs.User;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Sliders;
using Shoppe.Domain.Entities.Tags;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace Shoppe.Persistence.Concretes.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ITagReadRepository _tagReadRepository;
        private readonly IContentUpdater _contentUpdater;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly IPaginationService _paginationService;
        private readonly IJwtSession _jwtSession;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IReactionService _reactionService;
        public BlogService(IBlogReadRepository blogReadRepository, IBlogWriteRepository blogWriteRepository, ICategoryReadRepository categoryReadRepository, IUnitOfWork unitOfWork, IStorageService storageService, IPaginationService paginationService, IJwtSession jwtSession, ITagReadRepository tagReadRepository, IContentUpdater contentUpdater, IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository, IReactionService reactionService)
        {
            _blogReadRepository = blogReadRepository;
            _blogWriteRepository = blogWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _paginationService = paginationService;
            _jwtSession = jwtSession;
            _tagReadRepository = tagReadRepository;
            _contentUpdater = contentUpdater;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _reactionService = reactionService;
        }

        public async Task ChangeCoverImageAsync(Guid blogId, Guid? newCoverImageId, IFormFile? newCoverImageFile, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            var blog = await _blogReadRepository.Table
                .Include(b => b.BlogCoverImageFile)
                .Include(b => b.ContentImages)
                .FirstOrDefaultAsync(b => b.Id == blogId, cancellationToken);


            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            BlogContentImageFile? newCoverImage = null;

            if (newCoverImageFile != null && newCoverImageFile.Length > 0)
            {
                var (path, fileName) = await _storageService.UploadAsync(BlogConst.ImagesFolder, newCoverImageFile);

                newCoverImage = new BlogContentImageFile
                {
                    FileName = fileName,
                    PathName = path,

                    Storage = _storageService.StorageName
                };
            }

            else if (newCoverImageId != null)
            {
                newCoverImage = blog.ContentImages.FirstOrDefault(i => i.Id == newCoverImageId);

                if (newCoverImage == null)
                {
                    throw new EntityNotFoundException(nameof(newCoverImage));
                }
            }

            else throw new ValidationException("No new cover image file is specified");

            blog.BlogCoverImageFile = newCoverImage;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task RemoveImageAsync(Guid blogId, Guid imageId, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var blog = await _blogReadRepository.Table
                .Include(b => b.BlogCoverImageFile)
                .Include(b => b.ContentImages)
                .ThenInclude(i => i.Blogs)
                .FirstOrDefaultAsync(b => b.Id == blogId, cancellationToken);

            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            var image = blog.ContentImages.FirstOrDefault(i => i.Id == imageId);

            if (image == null)
            {
                throw new EntityNotFoundException(nameof(image));
            }

            var blogCovers = _blogReadRepository.Table.Select(b => b.BlogCoverImageFile);

            if (blogCovers.Contains(image)) throw new DeleteNotSucceedException("Cannot remove cover image. First change it.");

            bool isRemoved = blog.ContentImages.Remove(image);

            if (isRemoved)
            {
                if (image.Blogs.Count == 0)
                {
                    if (_fileWriteRepository.Delete(image))
                        await _storageService.DeleteAsync(image.PathName, image.FileName);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                scope.Complete();
            }
        }

        public async Task CreateAsync(CreateBlogDTO createBlogDTO, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            var blog = new Blog
            {
                Title = createBlogDTO.Title,
                AuthorId = _jwtSession.GetUserId(),
            };
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (createBlogDTO.CoverImageFile != null && createBlogDTO.CoverImageFile.Length > 0)
            {

                var (path, fileName) = await _storageService.UploadAsync(BlogConst.ImagesFolder, createBlogDTO.CoverImageFile);

                blog.BlogCoverImageFile = new BlogContentImageFile
                {
                    FileName = fileName,
                    PathName = path,
                    Storage = _storageService.StorageName
                };
            }

            if (createBlogDTO.Categories.Count > 0)
            {
                foreach (var categoryName in createBlogDTO.Categories)
                {
                    var category = await _categoryReadRepository.GetAsync(c => c.Name == categoryName, cancellationToken);

                    if (category is BlogCategory blogCategory)
                    {
                        blog.Categories.Add(blogCategory);
                    }
                }
            }

            if (createBlogDTO.Tags.Count > 0)
            {
                foreach (var tagName in createBlogDTO.Tags)
                {
                    var tag = await _tagReadRepository.GetAsync(c => c.Name == tagName, cancellationToken);

                    if (tag is BlogTag blogTag)
                    {
                        blog.Tags.Add(blogTag);
                    }
                }
            }

            if (!string.IsNullOrEmpty(createBlogDTO.Content) && blog.Content != createBlogDTO.Content)
            {
                blog.Content = createBlogDTO.Content;
            }

            if (createBlogDTO.ContentImages != null && createBlogDTO.ContentImages.Count > 0)
            {
                foreach (var image in createBlogDTO.ContentImages)
                {
                    var (path, fileName) = await _storageService.UploadAsync(BlogConst.ImagesFolder, image.ImageFile);

                    blog.ContentImages.Add(new BlogContentImageFile
                    {
                        FileName = fileName,
                        PathName = path,
                        PreviewUrl = image.PreviewUrl,
                        Storage = _storageService.StorageName,
                    });
                }

                blog.Content = _contentUpdater.UpdateBlobUrlsInContent(blog.Content, blog.ContentImages)!;
            }

            bool isAdded = await _blogWriteRepository.AddAsync(blog, cancellationToken);

            if (!isAdded)
            {
                throw new AddNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task DeleteAsync(Guid blogId, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var blog = await _blogReadRepository.Table.Include(b => b.ContentImages).Include(b => b.BlogCoverImageFile).ThenInclude(i => i.Blogs).FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            bool isDeleted = _blogWriteRepository.Delete(blog);

            if (!isDeleted)
            {
                throw new DeleteNotSucceedException();
            }

            bool isRemoved = _fileWriteRepository.DeleteRange(blog.ContentImages);

            if (isRemoved)
            {
                await _storageService.DeleteMultipleAsync(blog.ContentImages);
            }

            if (blog.BlogCoverImageFile.Blogs.Count == 0)
            {
                if (_fileWriteRepository.Delete(blog.BlogCoverImageFile))
                    await _storageService.DeleteAsync(blog.BlogCoverImageFile.PathName, blog.BlogCoverImageFile.FileName);
            }


            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task<GetAllBlogsDTO> GetAllAsync(BlogFilterParamsDTO blogFilterParamsDTO, CancellationToken cancellationToken)
        {
            var blogQuery = _blogReadRepository.Table
                                    .Include(b => b.Author)
                                    .Include(b => b.Tags)
                                    .Include(b => b.Categories)
                                    .Include(b => b.ContentImages)
                                    .Include(b => b.Reactions)
                                    .Include(b => b.BlogCoverImageFile)
                                    .AsNoTrackingWithIdentityResolution()
                                    .AsQueryable();

            if (blogFilterParamsDTO.TagName is string tagName and not "all")
            {
                blogQuery = blogQuery.Where(b => b.Tags.Any(t => t.Name == tagName));
            }

            if (blogFilterParamsDTO.CategoryName is string categoryName and not "all")
            {
                blogQuery = blogQuery.Where(b => b.Categories.Any(c => c.Name == categoryName));
            }

            if (blogFilterParamsDTO.SearchQuery is string searchQuery and not "")
            {
                blogQuery = blogQuery.Where(b => b.Title.Contains(searchQuery) || b.Content.Contains(searchQuery));
            }

            var paginationResult = await _paginationService.ConfigurePaginationAsync(blogFilterParamsDTO.Page, blogFilterParamsDTO.PageSize, blogQuery, cancellationToken);

            var blogDtos = await paginationResult.PaginatedQuery.Select(blog => blog.ToGetBlogDTO(_reactionService)).ToListAsync(cancellationToken);

            return new GetAllBlogsDTO
            {
                Blogs = blogDtos,
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages
            };
        }

        public async Task<GetBlogDTO> GetAsync(Guid blogId, CancellationToken cancellationToken)
        {
            var blog = await _blogReadRepository.Table
                .Include(b => b.BlogCoverImageFile)
                .Include(b => b.Author)
                .Include(b => b.Tags)
                .Include(b => b.Categories)
                .Include(b => b.ContentImages)
                .Include(b => b.Reactions)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(b => b.Id == blogId, cancellationToken);


            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            return blog.ToGetBlogDTO(_reactionService);
        }

        public async Task UpdateAsync(UpdateBlogDTO updateBlogDTO, CancellationToken cancellationToken)
        {
            _jwtSession.ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var blog = await _blogReadRepository.Table
                .Include(b => b.Tags)
                .Include(b => b.Categories)
                .Include(b => b.ContentImages)
                .FirstOrDefaultAsync(b => b.Id == updateBlogDTO.BlogId, cancellationToken);


            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            if (updateBlogDTO.Title is string title && blog.Title != title)
            {
                blog.Title = title;
            }

            if (updateBlogDTO.Categories.Count > 0)
            {
                // Remove categories not in the updated list
                var categoriesToRemove = blog.Categories
                    .Where(existingCategory => !updateBlogDTO.Categories.Contains(existingCategory.Name))
                    .ToList();

                foreach (var category in categoriesToRemove)
                {
                    blog.Categories.Remove(category);
                }

                // Add new categories
                foreach (var categoryName in updateBlogDTO.Categories)
                {
                    var category = await _categoryReadRepository.GetAsync(c => c.Name == categoryName, cancellationToken);
                    if (category is BlogCategory BlogCategory && !blog.Categories.Contains(category))
                    {
                        blog.Categories.Add(BlogCategory);
                    }
                }
            }

            if (updateBlogDTO.Tags.Count > 0)
            {
                var tagsToRemove = blog.Tags
                    .Where(existingTag => !updateBlogDTO.Tags.Contains(existingTag.Name))
                    .ToList();

                foreach (var tag in tagsToRemove)
                {
                    blog.Tags.Remove(tag);
                }

                // Add new categories
                foreach (var tagName in updateBlogDTO.Tags)
                {
                    var tag = await _tagReadRepository.GetAsync(c => c.Name == tagName, cancellationToken);

                    if (tag is BlogTag blogTag && !blog.Tags.Contains(tag))
                    {
                        blog.Tags.Add(blogTag);
                    }
                }
            }

            if (!string.IsNullOrEmpty(updateBlogDTO.Content) && blog.Content != updateBlogDTO.Content)
            {
                blog.Content = updateBlogDTO.Content;
            }

            if (updateBlogDTO.ContentImages != null && updateBlogDTO.ContentImages.Count > 0)
            {
                foreach (var image in updateBlogDTO.ContentImages)
                {
                    var (path, fileName) = await _storageService.UploadAsync(BlogConst.ImagesFolder, image.ImageFile);

                    blog.ContentImages.Add(new BlogContentImageFile
                    {
                        FileName = fileName,
                        PathName = path,
                        PreviewUrl = image.PreviewUrl,
                        Storage = _storageService.StorageName,
                    });
                }

                blog.Content = _contentUpdater.UpdateBlobUrlsInContent(blog.Content, blog.ContentImages)!;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task<List<GetContentFileDTO>> GetAllBlogImagesAsync(CancellationToken cancellationToken, Guid? blogId = null)
        {
            if (blogId != null)
            {
                var blog = await _blogReadRepository.Table
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(b => b.Id == blogId, cancellationToken);

                if (blog == null)
                {
                    throw new EntityNotFoundException(nameof(blog));
                }

                await _blogReadRepository.Table.Entry(blog).Collection(b => b.ContentImages).LoadAsync(cancellationToken);

                return blog.ContentImages.Select(i => i.ToGetContentFileDTO()).ToList();
            }
            else
            {
                var images = await _fileReadRepository.Table.OfType<BlogContentImageFile>().Select(i => i.ToGetContentFileDTO()).ToListAsync(cancellationToken);

                return images;
            }
        }

    }
}
