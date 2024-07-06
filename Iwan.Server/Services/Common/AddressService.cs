using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Common;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Common;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Common
{
    [Injected(ServiceLifetime.Scoped, typeof(IAddressService))]
    public class AddressService : IAddressService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public AddressService(IUnitOfWork unitOfWork, ILoggedInUserProvider loggedInUserProvider)
        {
            _loggedInUserProvider = loggedInUserProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Address> AddAddressAsync(AddAddressDto addressToAdd, CancellationToken cancellationToken = default)
        {
            var address = new Address
            {
                City = addressToAdd.City,
                Company = addressToAdd.Company,
                Country = addressToAdd.Country,
                Email = addressToAdd.Email,
                PhoneNumber = addressToAdd.PhoneNumber,
                Address1 = addressToAdd.Address1,
                Address2 = addressToAdd.Address2,
            };

            await _unitOfWork.AddressesRepository.AddAsync(address, cancellationToken);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            return address;
        }

        public async Task DeleteAddressAsync(string addressId, CancellationToken cancellationToken = default)
        {
            var address = await _unitOfWork.AddressesRepository.FindAsync(addressId, cancellationToken);

            if (address == null)
                throw new NotFoundException(Responses.Addresses.AddressNotFound);

            _unitOfWork.AddressesRepository.Delete(address);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task<Address> EditAddressAsync(EditAddressDto editedAddress, CancellationToken cancellationToken = default)
        {
            var address = await _unitOfWork.AddressesRepository.FindAsync(editedAddress.Id, cancellationToken);

            if (address == null)
                throw new NotFoundException(Responses.Addresses.AddressNotFound);

            address.SetIfNotEqual(a => a.City, editedAddress.City);
            address.SetIfNotEqual(a => a.Company, editedAddress.Company);
            address.SetIfNotEqual(a => a.Country, editedAddress.Country);
            address.SetIfNotEqual(a => a.Email, editedAddress.Email);
            address.SetIfNotEqual(a => a.PhoneNumber, editedAddress.PhoneNumber);
            address.SetIfNotEqual(a => a.Address1, editedAddress.Address1);
            address.SetIfNotEqual(a => a.Address2, editedAddress.Address2);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            return address;
        }
    }
}
