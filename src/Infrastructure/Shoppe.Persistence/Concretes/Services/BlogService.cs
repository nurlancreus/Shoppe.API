using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.BlogImageFileRepos;
using Shoppe.Application.Abstractions.Repositories.BlogRepos;
using Shoppe.Application.Abstractions.Repositories.CategoryRepos;
using Shoppe.Application.Abstractions.Repositories.SectionRepos;
using Shoppe.Application.Abstractions.Repositories.TagRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.Constants;
using Shoppe.Application.DTOs.Blog;
using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reply;
using Shoppe.Application.DTOs.Review;
using Shoppe.Application.DTOs.Section;
using Shoppe.Application.DTOs.User;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Entities.Categories;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Sections;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Shoppe.Persistence.Concretes.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly ISectionWriteRepository _sectionWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ITagReadRepository _tagReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly IPaginationService _paginationService;
        private readonly IJwtSession _jwtSession;
        private readonly IBlogImageFileReadRepository _blogImageFileReadRepository;

        public BlogService(IBlogReadRepository blogReadRepository, IBlogWriteRepository blogWriteRepository, ICategoryReadRepository categoryReadRepository, IUnitOfWork unitOfWork, IStorageService storageService, IPaginationService paginationService, IJwtSession jwtSession, ITagReadRepository tagReadRepository, IBlogImageFileReadRepository blogImageFileReadRepository, ISectionWriteRepository sectionWriteRepository)
        {
            _blogReadRepository = blogReadRepository;
            _blogWriteRepository = blogWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _paginationService = paginationService;
            _jwtSession = jwtSession;
            _tagReadRepository = tagReadRepository;
            _blogImageFileReadRepository = blogImageFileReadRepository;
            _sectionWriteRepository = sectionWriteRepository;
        }

        public async Task ChangeCoverImageAsync(string blogId, string? newCoverImageId, IFormFile? newCoverImageFile, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            var blog = await _blogReadRepository.Table
                .Include(b => b.BlogCoverImageFile)
                .Include(b => b.Sections)
                    .ThenInclude(s => s.BlogImageMappings)
                        .ThenInclude(bi => bi.BlogImage)
                .FirstOrDefaultAsync(b => b.Id.ToString() == blogId, cancellationToken);


            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            BlogImageFile? newCoverImage = null;

            if (!string.IsNullOrWhiteSpace(newCoverImageId))
            {
                newCoverImage = blog.Sections.SelectMany(s => s.BlogImageMappings).FirstOrDefault(bi => bi.BlogImage.Id.ToString().ToLower() == newCoverImageId.ToLower())?.BlogImage;

                if (newCoverImage == null)
                {
                    throw new EntityNotFoundException(nameof(newCoverImage));
                }
            }
            else if (newCoverImageFile != null && newCoverImageFile.Length > 0)
            {
                var (path, fileName) = await _storageService.UploadAsync(BlogConst.ImagesFolder, newCoverImageFile);

                newCoverImage = new BlogImageFile
                {
                    FileName = fileName,
                    PathName = path,
                    Storage = _storageService.StorageName
                };
            }
            else throw new ValidationException("No new cover image file is specified");

            blog.BlogCoverImageFile = newCoverImage;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

        public async Task RemoveImageAsync(string blogId, string imageId, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var blog = await _blogReadRepository.Table
                .Include(b => b.Sections)
                    .ThenInclude(s => s.BlogImageMappings)
                    .ThenInclude(bi => bi.BlogImage)
                .FirstOrDefaultAsync(b => b.Id.ToString() == blogId, cancellationToken);

            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            var image = blog.Sections.SelectMany(s => s.BlogImageMappings).FirstOrDefault(bi => bi.BlogImageId.ToString() == imageId);

            if (image == null)
            {
                throw new EntityNotFoundException(nameof(image));
            }

            bool isRemoved = image.BlogSection.BlogImageMappings.Remove(image);

            if (isRemoved)
            {
                if (image.BlogImage.Blogs.Count == 0 && image.BlogImage.BlogMappings.Count == 0)
                {
                    await _storageService.DeleteAsync(image.BlogImage.PathName, image.BlogImage.FileName);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                scope.Complete();
            }
        }

        public async Task CreateAsync(CreateBlogDTO createBlogDTO, CancellationToken cancellationToken)
        {
            ValidateAdminAccess();

            var blog = new Blog
            {
                Title = createBlogDTO.Title,
            };
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (createBlogDTO.CoverImageFile != null && createBlogDTO.CoverImageFile.Length > 0)
            {

                var (path, fileName) = await _storageService.UploadAsync(BlogConst.ImagesFolder, createBlogDTO.CoverImageFile);

                blog.BlogCoverImageFile = new BlogImageFile
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

            if (createBlogDTO.Sections.Count > 0)
            {
                foreach (var section in createBlogDTO.Sections)
                {
                    List<BlogBlogImage> uploadedImageMappings = [];

                    if (section.SectionImageFiles.Count > 0)
                    {
                        var uploadResults = await _storageService.UploadMultipleAsync(BlogConst.ImagesFolder, section.SectionImageFiles);

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
                            });

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
                                    .Include(b => b.Author)
                                    .AsNoTrackingWithIdentityResolution()
                                    .AsQueryable();

            var paginationResult = await _paginationService.ConfigurePaginationAsync(page, pageSize, blogQuery, cancellationToken);

            var blogDtos = await blogQuery.Select(blog => new GetBlogDTO
            {
                Id = blog.Id.ToString(),
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
                    ImageFiles = s.BlogImageMappings.Select(bi => new GetImageFileDTO
                    {
                        Id = bi.BlogImage.Id.ToString(),
                        FileName = bi.BlogImage.FileName,
                        PathName = bi.BlogImage.PathName,
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
                .AsNoTrackingWithIdentityResolution()
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
                    ImageFiles = s.BlogImageMappings.Select(bi => new GetImageFileDTO
                    {
                        Id = bi.BlogImage.Id.ToString(),
                        FileName = bi.BlogImage.FileName,
                        PathName = bi.BlogImage.PathName,
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
                .Include(b => b.Tags)
                .Include(b => b.Categories)
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

            var usedOrders = new HashSet<byte>(blog.Sections.Select(s => s.Order));

            if (updateBlogDTO.UpdatedSections.Count == 0)
            {
                if (_sectionWriteRepository.DeleteRange(blog.Sections))
                {
                    var blogImages = blog.Sections.SelectMany(s => s.BlogImageMappings).Select(bi => bi.BlogImage);

                    blog.Sections.Clear();

                    foreach (var image in blogImages)
                    {
                        if (image.Blogs.Count == 0 && image.BlogMappings.Count == 0)
                        {
                            await _storageService.DeleteAsync(image.PathName, image.PathName);
                        }
                    }
                }
            }
            else
            {
                if (updateBlogDTO.UpdatedSections.Count > blog.Sections.Count)
                {
                    var sectionsToDelete = blog.Sections.Where(s => !updateBlogDTO.UpdatedSections.Select(s => s.SectionId).Contains(s.Id.ToString()));

                    if (_sectionWriteRepository.DeleteRange(sectionsToDelete))
                    {
                        var blogImages = sectionsToDelete.SelectMany(s => s.BlogImageMappings).Select(bi => bi.BlogImage);

                        foreach (var section in sectionsToDelete)
                        {
                            blog.Sections.Remove(section);
                        }

                        foreach (var image in blogImages)
                        {
                            if (image.Blogs.Count == 0 && image.BlogMappings.Count == 0)
                            {
                                await _storageService.DeleteAsync(image.PathName, image.PathName);
                            }
                        }
                    }
                }

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

                    if (section.Order is byte order && order >= 0 && order <= 255 && existingSection.Order != order)
                    {
                        byte resolvedOrder = order;
                        while (usedOrders.Contains(resolvedOrder))
                        {
                            resolvedOrder++;
                        }

                        usedOrders.Add(resolvedOrder);

                        existingSection.Order = resolvedOrder;
                    }

                    if (section.SectionImageFiles.Count > 0)
                    {

                        var uploadResults = await _storageService.UploadMultipleAsync(BlogConst.ImagesFolder, section.SectionImageFiles);

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
                            });

                        }

                        existingSection.BlogImageMappings = uploadedImageMappings;

                    }
                }
            }

            foreach (var section in updateBlogDTO.NewSections)
            {
                List<BlogBlogImage> uploadedImageMappings = [];

                if (section.SectionImageFiles.Count > 0)
                {
                    var uploadResults = await _storageService.UploadMultipleAsync(BlogConst.ImagesFolder, section.SectionImageFiles);

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
                        });

                    }

                }

                byte resolvedOrder = section.Order;

                while (usedOrders.Contains(resolvedOrder))
                {
                    resolvedOrder++;
                }

                usedOrders.Add(resolvedOrder);

                var blogSection = new BlogSection
                {
                    Title = section.Title,
                    Description = section.Description,
                    TextBody = section.TextBody,
                    Order = resolvedOrder,
                    BlogImageMappings = uploadedImageMappings,
                };

                blog.Sections.Add(blogSection);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task<List<GetImageFileDTO>> GetAllBlogImagesAsync(CancellationToken cancellationToken, string? blogId = null)
        {
            if (blogId != null)
            {
                var blog = await _blogReadRepository.Table
                .Include(b => b.Sections)
                    .ThenInclude(s => s.BlogImageMappings)
                        .ThenInclude(bi => bi.BlogImage)
                            .ThenInclude(i => i.Blogs)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(b => b.Id.ToString() == blogId, cancellationToken);

                if (blog == null)
                {
                    throw new EntityNotFoundException(nameof(blog));
                }

                var imageMappings = blog.Sections.SelectMany(s => s.BlogImageMappings.Where(bi => bi.BlogImage.Blogs.Count == 0));

                return imageMappings.Select(i => new GetImageFileDTO
                {
                    Id = i.BlogImageId.ToString(),
                    PathName = i.BlogImage.PathName,
                    FileName = i.BlogImage.FileName,
                    CreatedAt = i.BlogImage.CreatedAt,
                }).ToList();
            }
            else
            {
                var blogImages = await _blogImageFileReadRepository.Table.Include(bi => bi.Blogs).Where(bi => bi.Blogs.Count == 0).AsNoTracking().ToListAsync(cancellationToken);

                return blogImages.Select(i => new GetImageFileDTO
                {
                    Id = i.Id.ToString(),
                    PathName = i.PathName,
                    FileName = i.FileName,
                    CreatedAt = i.CreatedAt,
                }).ToList();
            }
        }

        public async Task<List<GetReplyDTO>> GetRepliesByBlogAsync(string blogId, CancellationToken cancellationToken)
        {
            var blog = await _blogReadRepository.Table.Include(b => b.Replies).ThenInclude(r => r.Replier).ThenInclude(u => u.ProfilePictureFiles).FirstOrDefaultAsync(b => b.Id.ToString() == blogId, cancellationToken);

            if (blog == null)
            {
                throw new EntityNotFoundException(nameof(blog));
            }

            return blog.Replies.Select(r =>
            {
                var profilePic = r.Replier.ProfilePictureFiles.FirstOrDefault(i => i.IsMain);

                return new GetReplyDTO()
                {
                    Id = r.Id.ToString(),
                    FirstName = r.Replier?.FirstName!,
                    LastName = r.Replier?.LastName!,
                    Body = r.Body,
                    CreatedAt = r.CreatedAt,
                    ProfilePhoto = profilePic != null ? new GetImageFileDTO
                    {
                        Id = profilePic.Id.ToString(),
                        FileName = profilePic.FileName,
                        PathName = profilePic.PathName,
                        CreatedAt = profilePic.CreatedAt
                    } : null
                };


            }).ToList();
        }
        private void ValidateAdminAccess()
        {
            if (!_jwtSession.IsAdmin())
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

    }
}
