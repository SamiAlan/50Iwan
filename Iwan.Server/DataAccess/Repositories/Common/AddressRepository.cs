using Iwan.Server.Domain.Common;

namespace Iwan.Server.DataAccess.Repositories.Common
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context) : base(context) { }
    }
}
