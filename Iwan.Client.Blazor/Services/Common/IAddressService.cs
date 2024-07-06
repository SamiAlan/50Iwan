using Iwan.Shared.Dtos.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Common
{
    public interface IAddressService
    {
        Task<AddressDto> GetAddressAsync(string addressId, CancellationToken cancellationToken = default);
        Task<AddressDto> EditAddressAsync(EditAddressDto editedAddress, CancellationToken cancellationToken = default);
    }
}
