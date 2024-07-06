using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Common;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Dtos.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Common
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryAddressService))]
    public class QueryAddressService : IQueryAddressService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public QueryAddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressDto> GetAddressDetailsAsync(string addressId, CancellationToken cancellationToken = default)
        {
            var address = await _unitOfWork.AddressesRepository.FindAsync(addressId, cancellationToken);

            if (address == null)
                throw new NotFoundException(Responses.Addresses.AddressNotFound);

            return new AddressDto
            {
                Id = address.Id,
                City = address.City,
                Country = address.Country,
                Company = address.Company,
                Email = address.Email,
                PhoneNumber = address.PhoneNumber,
                Address1 = address.Address1,
                Address2 = address.Address2
            };
        }
    }
}
