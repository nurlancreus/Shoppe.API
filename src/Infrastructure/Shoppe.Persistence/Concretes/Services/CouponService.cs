using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Application.Abstractions.Repositories.CouponRepos;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Coupon;
using Shoppe.Application.Extensions.Mapping;
using Shoppe.Domain.Entities;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponReadRepository _couponReadRepository;
        private readonly ICouponWriteRepository _couponWriteRepository;
        private readonly IJwtSession _jwtSession;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;

        public CouponService(ICouponReadRepository couponReadRepository, ICouponWriteRepository couponWriteRepository, IJwtSession jwtSession, IUnitOfWork unitOfWork, IPaginationService paginationService)
        {
            _couponReadRepository = couponReadRepository;
            _couponWriteRepository = couponWriteRepository;
            _jwtSession = jwtSession;
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
        }

        public async Task<GetCouponDTO> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var coupon = await _couponReadRepository.Table
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (coupon == null)
            {
                throw new EntityNotFoundException(nameof(coupon));
            }

            return coupon.ToGetCouponDTO();

        }

        public async Task<GetAllCouponsDTO> GetAll(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            _jwtSession.ValidateAdminAccess();

            var query = _couponReadRepository.Table
                        .AsNoTracking();

            var paginatedQuery = await _paginationService.ConfigurePaginationAsync(page, pageSize, query, cancellationToken);

            return new GetAllCouponsDTO
            {
                Page = paginatedQuery.Page,
                PageSize = paginatedQuery.PageSize,
                TotalItems = paginatedQuery.TotalItems,
                TotalPages = paginatedQuery.TotalPages,
                Coupons = paginatedQuery.PaginatedQuery.Select(c => c.ToGetCouponDTO()).ToList()
            };
        }
        public async Task CreateAsync(CreateCouponDTO createCouponDTO, CancellationToken cancellationToken = default)
        {
            _jwtSession.ValidateAdminAccess();

            if (ICouponValidationService.CheckIfCodeValid(createCouponDTO.Code, true))
            {
                bool isCodeExist = await _couponReadRepository.IsExistAsync(c => c.Code == createCouponDTO.Code, cancellationToken);

                if (isCodeExist)
                {
                    throw new AddNotSucceedException($"Coupon with code {createCouponDTO.Code} is already exists");
                }

                var coupon = new Coupon
                {
                    Code = createCouponDTO.Code,
                    MaxUsage = createCouponDTO.MaxUsage,
                    MinimumOrderAmount = createCouponDTO.MinimumOrderAmount,
                    StartDate = createCouponDTO.StartDate,
                    EndDate = createCouponDTO.EndDate,
                    DiscountPercentage = createCouponDTO.DiscountPercentage,
                    IsActive = true,
                    UsageCount = 0,
                };

                bool isAdded = await _couponWriteRepository.AddAsync(coupon, cancellationToken);

                if (!isAdded) throw new AddNotSucceedException();

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var coupon = await _couponReadRepository.Table
                                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (coupon == null)
            {
                throw new EntityNotFoundException(nameof(coupon));
            }

            var isDeleted = _couponWriteRepository.Delete(coupon);

            if (!isDeleted) throw new DeleteNotSucceedException();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task ToggleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _jwtSession.ValidateAdminAccess();

            var coupon = await _couponReadRepository.Table
                                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (coupon == null)
            {
                throw new EntityNotFoundException(nameof(coupon));
            }

            bool isCouponValid = ICouponValidationService.CheckIfIsValid(coupon);

            if (isCouponValid)
            {
                coupon.IsActive = !coupon.IsActive;
            }
            else
            {
                if (coupon.IsActive) coupon.IsActive = false;
                else throw new UpdateNotSucceedException("Coupond is not valid.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UpdateCouponDTO updateCouponDTO, CancellationToken cancellationToken = default)
        {
            _jwtSession.ValidateAdminAccess();

            var coupon = await _couponReadRepository.Table
                                .FirstOrDefaultAsync(c => c.Id == updateCouponDTO.Id, cancellationToken);

            if (coupon == null)
            {
                throw new EntityNotFoundException(nameof(coupon));
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!string.IsNullOrEmpty(updateCouponDTO.Code) && coupon.Code != updateCouponDTO.Code)
            {
                if (ICouponValidationService.CheckIfCodeValid(updateCouponDTO.Code, true))
                    coupon.Code = updateCouponDTO.Code;
            }

            if (updateCouponDTO.MinimumOrderAmount is double minimumAmount && coupon.MinimumOrderAmount != minimumAmount)
            {
                coupon.MinimumOrderAmount = minimumAmount;
            }

            if (updateCouponDTO.DiscountPercentage is decimal discountPercentage && coupon.DiscountPercentage != discountPercentage)
            {
                coupon.DiscountPercentage = discountPercentage;
            }

            if (updateCouponDTO.MaxUsage is int maxUsage && coupon.MaxUsage != maxUsage)
            {
                if (maxUsage < coupon.UsageCount)
                {
                    throw new ValidationException("Max usage count is not valid");
                }

                coupon.MaxUsage = maxUsage;
            }

            if (updateCouponDTO.StartDate is DateTime startDate && coupon.StartDate != startDate)
            {
                if (DateTime.UtcNow >= startDate && coupon.EndDate <= startDate)
                {
                    throw new ValidationException("Start date is not valid");
                }
            }

            if (updateCouponDTO.EndDate is DateTime endDate && coupon.EndDate != endDate)
            {
                if (DateTime.UtcNow >= endDate && coupon.StartDate >= endDate)
                {
                    throw new ValidationException("End date is not valid");
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }
    }
}
