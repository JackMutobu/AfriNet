using AfriNetLocalApi.Entities;
using LanguageExt.Common;

namespace AfriNetLocalApi.Services.Bundles
{
    public interface IBundleService
    {
        Task<Bundle> Create(Bundle bundle, CancellationToken cancellationToken);
        Task<List<Bundle>> GetList(int skip, int take, CancellationToken cancellationToken);
        Task<Either<Bundle, Error>> Update(Bundle bundle, CancellationToken cancellationToken);
    }

    public class BundleService : IBundleService
    {
        private readonly AfriNetLocalDbContext _dbContext;
        public BundleService(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Bundle> Create(Bundle bundle, CancellationToken cancellationToken)
        {
            var result = await _dbContext.AddAsync(bundle, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public async Task<Either<Bundle, Error>> Update(Bundle bundle, CancellationToken cancellationToken)
        {
            var savedBundle = await _dbContext.Bundles.FirstOrDefaultAsync(x => x.Id == bundle.Id, cancellationToken);
            if (savedBundle is null)
                return Error.New("Bundle not found");
            _dbContext.Bundles.Update(bundle);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return savedBundle;
        }

        public Task<List<Bundle>> GetList(int skip, int take, CancellationToken cancellationToken)
        => _dbContext.Bundles.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }
}
