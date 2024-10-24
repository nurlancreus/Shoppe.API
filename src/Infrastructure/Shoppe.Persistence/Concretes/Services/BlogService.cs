using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Blog;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.User;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Sections;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly IPaginationService _paginationService;
        private readonly IJwtSession _jwtSession;

        public BlogService(IBlogReadRepository blogReadRepository, IBlogWriteRepository blogWriteRepository, ICategoryReadRepository categoryReadRepository, IUnitOfWork unitOfWork, IStorageService storageService, IPaginationService paginationService, IJwtSession jwtSession)
        {
            _blogReadRepository = blogReadRepository;
            _blogWriteRepository = blogWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _paginationService = paginationService;
            _jwtSession = jwtSession;
        }

        public async Task ChangeMainImageAsync(string blogId, string newMainImageId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            var blog = await _blogReadRepository.Table
                .Include(b => b.Sections)
                    .ThenInclude(s => s.BlogImageMappings)
                        .ThenInclude(bi => bi.BlogImage)
                .FirstOrDefaultAsync(b => b.Id.ToString() == blogId, cancellationToken);


            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            bool anyImageExist = false;

            if (blog.Sections.Count > 0)
            {
                foreach (var section in blog.Sections)
                {
                    if (section.BlogImageMappings.Count > 0)
                    {
                        anyImageExist = true;
                    }
                }

            }

            if (!anyImageExist)
            {
                throw new EntityNotFoundException("blog currently has no image.");
            }

            var existingMainImage = blog.Sections.SelectMany( s => s.BlogImageMappings).FirstOrDefault(i => i.IsMain);

            var newMainImage = blog.Sections.SelectMany(s => s.BlogImageMappings).FirstOrDefault(bi => bi.BlogImage.Id.ToString().ToLower() == newMainImageId.ToLower());

            if (newMainImage == null)
            {
                throw new EntityNotFoundException(nameof(newMainImage));
            }

            if (existingMainImage == newMainImage) return;

            if (existingMainImage != null)
            {
                existingMainImage.IsMain = false;

                // temporary solution!
                // await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            newMainImage.IsMain = true;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public Task RemoveImageAsync(string blogId, string newMainImageId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            throw new NotImplementedException();
        }

        public async Task CreateAsync(CreateBlogDTO createBlogDTO, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            var blog = new Blog
            {
                Title = createBlogDTO.Title,
            };
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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

            if (createBlogDTO.Sections.Count > 0)
            {
                foreach (var section in createBlogDTO.Sections)
                {
                    List<BlogBlogImage> uploadedImageMappings = [];

                    if (section.SectionImageFiles.Count > 0)
                    {
                        var uploadResults = await _storageService.UploadMultipleAsync(BlogConst.ImagesFolder, section.SectionImageFiles);

                        bool isFirst = true;
                        foreach (var (path, fileName) in uploadResults)
                        {

                            uploadedImageMappings.Add(new BlogBlogImage
                            {
                                BlogImage = new BlogImageFile
                                {
                                    FileName = fileName,
                                    PathName = path,
                                    Storage = _storageService.StorageName,
                                },
                                IsMain = isFirst,
                                BlogId = blog.Id
                            });

                            isFirst = false;
                        }

                    }

                    var blogSection = new BlogSection
                    {
                        Title = section.Title,
                        Description = section.Description,
                        TextBody = section.TextBody,
                        Order = section.Order,
                        BlogImageMappings = uploadedImageMappings,
                    };

                    blog.Sections.Add(blogSection);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task DeleteAsync(string blogId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var blog = await _blogReadRepository.GetByIdAsync(blogId, cancellationToken);

            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            bool isDeleted = _blogWriteRepository.Delete(blog);

            if (!isDeleted)
            {
                throw new DeleteNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task<GetAllBlogsDTO> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var blogQuery = _blogReadRepository.Table
                                    .Include(b => b.Sections)
                                        .ThenInclude(s => s.BlogImageMappings)
                                            .ThenInclude(bi => bi.BlogImage)
                                    .Include(b => b.Author).AsQueryable();

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, blogQuery, cancellationToken);

            var blogDtos = await blogQuery.Select(blog => new GetBlogDTO
            {
                Id = blog.Id.ToString(),
                Author = new Application.DTOs.User.GetUserDTO
                {
                    Id = blog.Author.Id,
                    FirstName = blog.Author.FirstName!,
                    LastName = blog.Author.LastName!,
                    Email = blog.Author.Email!,
                    UserName = blog.Author.UserName!,
                    CreatedAt = blog.Author.CreatedAt
                },
                Title = blog.Title,
                Sections = blog.Sections.Select(s => new Application.DTOs.Section.GetSectionDTO
                {
                    Id = s.Id.ToString(),
                    Title = s.Title,
                    Description = s.Description,
                    TextBody = s.TextBody,
                    SectionImageFiles = s.BlogImageMappings.Select(bi => new Application.DTOs.Files.GetImageFileDTO
                    {
                        Id = bi.BlogImage.Id.ToString(),
                        FileName = bi.BlogImage.FileName,
                        PathName = bi.BlogImage.PathName,
                        IsMain = bi.IsMain,
                        CreatedAt = bi.BlogImage.CreatedAt
                    }).ToList(),
                    Order = s.Order,
                    CreatedAt = s.CreatedAt
                }).ToList(),
                CreatedAt = blog.CreatedAt
            }).ToListAsync(cancellationToken);

            return new GetAllBlogsDTO
            {
                Blogs = blogDtos,
                Page = paginationResult.Page,
                PageSize = paginationResult.PageSize,
                TotalItems = paginationResult.TotalItems,
                TotalPages = paginationResult.TotalPages
            };
        }

        public async Task<GetBlogDTO> GetAsync(string blogId, CancellationToken cancellationToken)
        {
            var blog = await _blogReadRepository.Table
                .Include(b => b.Sections)
                    .ThenInclude(s => s.BlogImageMappings)
                        .ThenInclude(bi => bi.BlogImage)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id.ToString() == blogId, cancellationToken);


            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            return new GetBlogDTO
            {
                Id = blogId,
                Author = new GetUserDTO
                {
                    Id = blog.Author.Id,
                    FirstName = blog.Author.FirstName!,
                    LastName = blog.Author.LastName!,
                    Email = blog.Author.Email!,
                    UserName = blog.Author.UserName!,
                    CreatedAt = blog.Author.CreatedAt
                },
                Title = blog.Title,
                Sections = blog.Sections.Select(s => new GetSectionDTO
                {
                    Id = s.Id.ToString(),
                    Title = s.Title,
                    Description = s.Description,
                    TextBody = s.TextBody,
                    SectionImageFiles = s.BlogImageMappings.Select(bi => new GetImageFileDTO
                    {
                        Id = bi.BlogImage.Id.ToString(),
                        FileName = bi.BlogImage.FileName,
                        PathName = bi.BlogImage.PathName,
                        IsMain = bi.IsMain,
                        CreatedAt = bi.BlogImage.CreatedAt
                    }).ToList(),
                    Order = s.Order,
                    CreatedAt = s.CreatedAt
                }).ToList(),
                CreatedAt = blog.CreatedAt
            };
        }

        public async Task UpdateAsync(UpdateBlogDTO updateBlogDTO, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var blog = await _blogReadRepository.Table
                .Include(b => b.Sections)
                    .ThenInclude(s => s.BlogImageMappings)
                        .ThenInclude(bi => bi.BlogImage)
                 .FirstOrDefaultAsync(b => b.Id.ToString() == updateBlogDTO.BlogId, cancellationToken);


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

            if (updateBlogDTO.Sections.Count > 0)
            {
                var isMainImageExist = blog.Sections.Any(s => s.BlogImageMappings.Any(bi => bi.IsMain));

                foreach (var section in updateBlogDTO.Sections)
                {
                    List<BlogBlogImage> uploadedImageMappings = [];

                    if (section.SectionImageFiles.Count > 0)
                    {
                        var uploadResults = await _storageService.UploadMultipleAsync(BlogConst.ImagesFolder, section.SectionImageFiles);

                        bool isFirst = true;
                        foreach (var (path, fileName) in uploadResults)
                        {

                            uploadedImageMappings.Add(new BlogBlogImage
                            {
                                BlogImage = new BlogImageFile
                                {
                                    FileName = fileName,
                                    PathName = path,
                                    Storage = _storageService.StorageName,

                                },
                                IsMain = isFirst && !isMainImageExist,
                                BlogId = blog.Id
                            });

                            isFirst = false;
                        }

                    }

                    var blogSection = new BlogSection
                    {
                        Title = section.Title,
                        Description = section.Description,
                        TextBody = section.TextBody,
                        Order = section.Order,
                        BlogImageMappings = uploadedImageMappings,
                    };

                    blog.Sections.Add(blogSection);
                }
            }

            if (updateBlogDTO.UpdatedSections.Count > 0)
            {

                foreach (var section in updateBlogDTO.UpdatedSections)
                {
                    var existingSection = blog.Sections.FirstOrDefault(s => s.Id.ToString() == section.SectionId);

                    if (existingSection == null)
                    {
                        throw new EntityNotFoundException(nameof(existingSection));
                    }

                    if (section.Title is string sectionTitle && existingSection.Title != sectionTitle)
                    {
                        existingSection.Title = sectionTitle;
                    }

                    if (section.Description is string sectionDesc && existingSection.Title != sectionDesc)
                    {
                        existingSection.Description = sectionDesc;
                    }

                    if (section.TextBody is string text && existingSection.Title != text)
                    {
                        existingSection.TextBody = text;
                    }

                    if (section.Order is byte order && existingSection.Order != order)
                    {
                        existingSection.Order = order;
                    }

                    if (section.SectionImageFiles.Count > 0)
                    {
                        bool isMainImageExist = existingSection.BlogImageMappings.Any(bi => bi.IsMain);

                        var uploadResults = await _storageService.UploadMultipleAsync(BlogConst.ImagesFolder, section.SectionImageFiles);

                        bool isFirst = true;
                        List<BlogBlogImage> uploadedImageMappings = [];

                        foreach (var (path, fileName) in uploadResults)
                        {

                            uploadedImageMappings.Add(new BlogBlogImage
                            {
                                BlogImage = new BlogImageFile
                                {
                                    FileName = fileName,
                                    PathName = path,
                                    Storage = _storageService.StorageName,
                                },
                                IsMain = isFirst && !isMainImageExist,
                            });

                            isFirst = false;
                        }

                        existingSection.BlogImageMappings = uploadedImageMappings;

                    }

                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        private void ValidateAdminAccess()
        {
            if (!_jwtSession.IsAdmin())
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }
    }
}
