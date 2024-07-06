using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Common;
using Iwan.Server.Domain.Vendors;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Vendors
{
    [Injected(ServiceLifetime.Scoped, typeof(IVendorService))]
    public class VendorService : IVendorService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IQueryVendorService _queryVendorService;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public VendorService(IUnitOfWork unitOfWork, ILoggedInUserProvider loggedInUserProvider, IQueryVendorService queryVendorService)
        {
            _unitOfWork = unitOfWork;
            _loggedInUserProvider = loggedInUserProvider;
            _queryVendorService = queryVendorService;
        }

        public async Task<Vendor> AddVendorAsync(AddVendorDto vendorToAdd, CancellationToken cancellationToken = default)
        {
            var vendorWithSameNameExist = await _unitOfWork.VendorsRepository.AnyAsync(v => v.Name.ToLower() == vendorToAdd.Name.ToLower(), cancellationToken);

            if (vendorWithSameNameExist)
                throw new AlreadyExistException(Responses.Vendors.SameNameAlreadyExist, 
                    vendorToAdd.GetPropertyPath(v => v.Name));

            var vendor = new Vendor
            {
                Name = vendorToAdd.Name,
                BenefitPercent = vendorToAdd.BenefitPercent,
                ShowOwnProducts = vendorToAdd.ShowOwnProducts
            };

            var addressToAdd = vendorToAdd.Address;

            var address = await _unitOfWork.AddressesRepository.AddAsync(new Address
            {
                City = addressToAdd.City,
                Country = addressToAdd.Country,
                Company = addressToAdd.Company,
                Address1 = addressToAdd.Address1,
                Address2 = addressToAdd.Address2,
                Email = addressToAdd.Email,
                PhoneNumber = addressToAdd.PhoneNumber
            }, cancellationToken);

            vendor.AddressId = address.Id;

            await _unitOfWork.VendorsRepository.AddAsync(vendor, cancellationToken);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return vendor;
        }

        public async Task DeleteVendorAsync(string vendorId, CancellationToken cancellationToken = default)
        {
            var vendor = await _unitOfWork.VendorsRepository.GetAsync(vendorId, true, cancellationToken);

            if (vendor == null)
                throw new NotFoundException(Responses.Vendors.VendorNotFound);

            var address = await _unitOfWork.AddressesRepository.FindAsync(vendor.AddressId, cancellationToken);

            _unitOfWork.AddressesRepository.Delete(address);
            _unitOfWork.VendorsRepository.Delete(vendor);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task<Vendor> EditVendorAsync(EditVendorDto editedVendor, CancellationToken cancellationToken = default)
        {
            var vendor = await _unitOfWork.VendorsRepository.FindAsync(editedVendor.Id, cancellationToken);

            if (vendor == null)
                throw new NotFoundException(Responses.Vendors.VendorNotFound);

            var vendorWithSameNameExist = await _unitOfWork.VendorsRepository.AnyAsync(v => v.Id != vendor.Id && v.Name.ToLower() == editedVendor.Name.ToLower(), cancellationToken);

            if (vendorWithSameNameExist)
                throw new AlreadyExistException(Responses.Vendors.SameNameAlreadyExist,
                    editedVendor.GetPropertyPath(v => v.Name));

            vendor.Name = editedVendor.Name;
            vendor.BenefitPercent = editedVendor.BenefitPercent;
            vendor.ShowOwnProducts = editedVendor.ShowOwnProducts;

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return vendor;
        }
    }
}
