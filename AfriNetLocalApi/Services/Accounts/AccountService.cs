using AfriNetLocalApi.Entities;

namespace AfriNetLocalApi.Services.Accounts
{
    public interface IAccountService
    {
        Task<Account> Create(Account account, CancellationToken cancellationToken);
    }

    public class AccountService : IAccountService
    {
        private readonly AfriNetLocalDbContext _dbContext;

        public AccountService(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> Create(Account account, CancellationToken cancellationToken)
        {
            var result = await _dbContext.AddAsync(account, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}
