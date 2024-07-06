using Iwan.Shared.Dtos.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Common
{
    public interface IQueryAddressService
    {
        Task<AddressDto> GetAddressDetailsAsync(string addressId, CancellationToken cancellationToken = default);
    }
}
