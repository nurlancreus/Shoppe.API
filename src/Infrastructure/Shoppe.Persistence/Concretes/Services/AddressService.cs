using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories.AddressRepos;
using Shoppe.Application.Abstractions.Services.Address;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Application.DTOs.Address;
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
    public class AddressService : IAddressService
    {
        private readonly IAddressReadRepository _addressReadRepository;
        private readonly IAddressWriteRepository _addressWriteRepository;
        private readonly IJwtSession _jwtSession;
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IAddressReadRepository addressReadRepository, IAddressWriteRepository addressWriteRepository, IUnitOfWork unitOfWork, IJwtSession jwtSession)
        {
            _addressReadRepository = addressReadRepository;
            _addressWriteRepository = addressWriteRepository;
            _unitOfWork = unitOfWork;
            _jwtSession = jwtSession;
        }

        public async Task CreateBillingAsync(CreateBillingAddressDTO createBillingAddressDTO, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var existingAddress = await _addressReadRepository.GetAsync(a =>
                a.StreetAddress == createBillingAddressDTO.StreetAddress &&
                a.City == createBillingAddressDTO.City &&
                a.Country == createBillingAddressDTO.Country &&
                a.PostalCode == createBillingAddressDTO.PostalCode &&
                a.FirstName == createBillingAddressDTO.FirstName &&
                a.LastName == createBillingAddressDTO.LastName &&
                a.Email == createBillingAddressDTO.Email,
                cancellationToken);

            if (existingAddress != null)
                throw new InvalidOperationException("An identical billing address already exists.");


            if (!IAddressValidationService.AllowedCountries.TryGetValue(createBillingAddressDTO.Country, out var value))
            {
                throw new InvalidOperationException("We can only ship to Azerbaijan or its neighboring countries.");
            }

            bool isPostalCodeValid = IAddressValidationService.ValidatePostalCode(createBillingAddressDTO.PostalCode, countryCode: value.CountryCode);

            if (!isPostalCodeValid)
            {
                throw new ValidationException("Invalid postal code for the specified country.");
            }

            var billingAddress = new BillingAddress()
            {
                FirstName = createBillingAddressDTO.FirstName,
                LastName = createBillingAddressDTO.LastName,
                Email = createBillingAddressDTO.Email,
                Phone = createBillingAddressDTO.Phone,
                City = createBillingAddressDTO.City,
                Country = createBillingAddressDTO.Country,
                PostalCode = createBillingAddressDTO.PostalCode,
                StreetAddress = createBillingAddressDTO.StreetAddress,
                UserId = userId
            };

            bool isAdded = await _addressWriteRepository.AddAsync(billingAddress, cancellationToken);

            if (!isAdded)
            {
                throw new AddNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateShippingAsync(CreateShippingAddressDTO createShippingAddressDTO, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            if (!IAddressValidationService.AllowedCountries.TryGetValue(createShippingAddressDTO.Country, out var value))
            {
                throw new InvalidOperationException("We can only ship to Azerbaijan or its neighboring countries.");
            }

            bool isPostalCodeValid = IAddressValidationService.ValidatePostalCode(createShippingAddressDTO.PostalCode, countryCode: value.CountryCode);

            if (!isPostalCodeValid)
            {
                throw new ValidationException("Invalid postal code for the specified country.");
            }

            var shippingAddress = new ShippingAddress()
            {
                FirstName = createShippingAddressDTO.FirstName,
                LastName = createShippingAddressDTO.LastName,
                Email = createShippingAddressDTO.Email,
                Phone = createShippingAddressDTO.Phone,
                City = createShippingAddressDTO.City,
                Country = createShippingAddressDTO.Country,
                PostalCode = createShippingAddressDTO.PostalCode,
                StreetAddress = createShippingAddressDTO.StreetAddress,
                UserId = userId,
            };

            bool isAdded = await _addressWriteRepository.AddAsync(shippingAddress, cancellationToken);

            if (!isAdded)
            {
                throw new AddNotSucceedException();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateBillingAsync(UpdateBillingAddressDTO updateBillingAddressDTO, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var address = await _addressReadRepository.Table
                .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (address is not BillingAddress billingAddress)
            {
                throw new EntityNotFoundException(nameof(address));
            }

            var existingAddress = await _addressReadRepository.IsExistAsync(a =>
                   a.FirstName == updateBillingAddressDTO.FirstName &&
                   a.LastName == updateBillingAddressDTO.LastName &&
                   a.Email == updateBillingAddressDTO.Email &&
                   a.StreetAddress == updateBillingAddressDTO.StreetAddress &&
                   a.City == updateBillingAddressDTO.City &&
                   a.Country == updateBillingAddressDTO.Country &&
                   a.PostalCode == updateBillingAddressDTO.PostalCode &&
                   a.UserId != userId, cancellationToken);

            if (existingAddress)
            {
                throw new InvalidOperationException("An identical billing address already exists.");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            UpdateFields(billingAddress, updateBillingAddressDTO);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        public async Task UpdateShippingAsync(UpdateShippingAddressDTO updateShippingAddressDTO, CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var address = await _addressReadRepository.Table
                .FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (address is not ShippingAddress shippingAddress)
            {
                throw new EntityNotFoundException(nameof(address));
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            UpdateFields(shippingAddress, updateShippingAddressDTO);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
        }

        private static void UpdateFields(Address address, UpdateAddressDTO updateAddressDTO)
        {

            if (!string.IsNullOrEmpty(updateAddressDTO.PostalCode) && address.PostalCode != updateAddressDTO.PostalCode)
            {
                if (!string.IsNullOrEmpty(updateAddressDTO.Country))
                {
                    if (!IAddressValidationService.AllowedCountries.TryGetValue(updateAddressDTO.Country, out var value))
                    {
                        throw new InvalidOperationException("We can only ship to Azerbaijan or its neighboring countries.");
                    }

                    var isPostalCodeValid = IAddressValidationService.ValidatePostalCode(updateAddressDTO.PostalCode, countryCode: value.CountryCode);

                    if (!isPostalCodeValid)
                    {
                        throw new ValidationException("The postal code is invalid for the given country.");
                    }

                    if (updateAddressDTO.Country != address.Country) address.Country = updateAddressDTO.Country;

                    address.PostalCode = updateAddressDTO.PostalCode;
                }

                else throw new InvalidOperationException("No Country specified");
            }

            if (!string.IsNullOrEmpty(updateAddressDTO.FirstName) && address.FirstName != updateAddressDTO.FirstName)
            {
                address.FirstName = updateAddressDTO.FirstName;
            }

            if (!string.IsNullOrEmpty(updateAddressDTO.LastName) && address.LastName != updateAddressDTO.LastName)
            {
                address.LastName = updateAddressDTO.LastName;
            }

            if (!string.IsNullOrEmpty(updateAddressDTO.Email) && address.Email != updateAddressDTO.Email)
            {
                address.Email = updateAddressDTO.Email;
            }

            if (!string.IsNullOrEmpty(updateAddressDTO.Phone) && address.Phone != updateAddressDTO.Phone)
            {
                address.Phone = updateAddressDTO.Phone;
            }

            if (!string.IsNullOrEmpty(updateAddressDTO.City) && address.City != updateAddressDTO.City)
            {
                address.City = updateAddressDTO.City;
            }

            if (!string.IsNullOrEmpty(updateAddressDTO.StreetAddress) && address.StreetAddress != updateAddressDTO.StreetAddress)
            {
                address.StreetAddress = updateAddressDTO.StreetAddress;
            }
        }

        public async Task<GetAddressDTO> GetBillingAddressAsync(CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var address = await _addressReadRepository.Table.OfType<BillingAddress>().AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (address == null)
            {
                throw new EntityNotFoundException(nameof(address));
            }

            return address.ToGetBillingAddressDTO();
        }

        public async Task<GetAddressDTO> GetShippingAddressAsync(CancellationToken cancellationToken = default)
        {
            var userId = _jwtSession.GetUserId();

            var address = await _addressReadRepository.Table.OfType<ShippingAddress>().AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

            if (address == null)
            {
                throw new EntityNotFoundException(nameof(address));
            }

            return address.ToGetShippingAddressDTO();
        }
    }
}
