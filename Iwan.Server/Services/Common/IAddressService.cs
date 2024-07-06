using Iwan.Server.Domain.Common;
using Iwan.Shared.Dtos.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Common
{
    public interface IAddressService
    {
        Task<Address> AddAddressAsync(AddAddressDto addressToAdd, CancellationToken cancellationToken = default);
        Task<Address> EditAddressAsync(EditAddressDto editedAddress, CancellationToken cancellationToken = default);
        Task DeleteAddressAsync(string addressId, CancellationToken cancellationToken = default);
    }
}
