using Iwan.Server.Domain.Security;

namespace Iwan.Server.DataAccess.Repositories.Security
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context) { }
    }
}
