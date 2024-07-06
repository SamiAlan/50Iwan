using Iwan.Server.Domain.Compositions;

namespace Iwan.Server.DataAccess.Repositories.Compositions
{
    public class CompositionRepository : Repository<Composition>, ICompositionRepository
    {
        public CompositionRepository(ApplicationDbContext context) : base(context) { }
    }
}
