using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;

namespace AfriNetLocalApi.Recharges.Services
{
    public record RechargeCommand(Guid AccountId, Guid BundleId, int Quantity, Guid? FromAccountId = null);
    public record RechargeResult(AccountTransaction Trasaction, ActiveBundle ActiveBundle);

    public interface IRechargeService
    {
        Task<IEnumerable<RechargeResult>> RechargeClient(RechargeCommand rechargeData, CancellationToken cancellationToken);
        Task<IEnumerable<AccountTransaction>> RechargeRetailer(RechargeCommand rechargeData, CancellationToken cancellationToken);
    }

    public class RechargeService : IRechargeService
    {
        private readonly AfriNetLocalDbContext _dbContext;

        public RechargeService(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AccountTransaction>> RechargeRetailer(RechargeCommand rechargeData, CancellationToken cancellationToken)
        {
            var results = new List<AccountTransaction>();
            foreach (var qty in Enumerable.Range(1, rechargeData.Quantity))
            {
                var result = await RechargeRetailer(rechargeData.AccountId, rechargeData.BundleId, rechargeData.FromAccountId, cancellationToken);
                results.Add(result);
            }
            return results;
        }

        Task<AccountTransaction> RechargeRetailer(Guid toAccountId, Guid bundleId, Guid? fromAccountId, CancellationToken cancellationToken)
        => CreateAndSaveTransaction(
            fromAccountId is null ? GetSuperDealerAcount() : GetAccount(fromAccountId ?? Guid.Empty),
            GetAccount(toAccountId),
            GetBundle(bundleId), cancellationToken)
          .MapAsync(data => UpdateAccounts(data, cancellationToken))
          .Map(x => x.Trasaction);

        public async Task<IEnumerable<RechargeResult>> RechargeClient(RechargeCommand rechargeData, CancellationToken cancellationToken)
        {
            var results = new List<RechargeResult>();
            foreach (var qty in Enumerable.Range(1, rechargeData.Quantity))
            {
                var result = await RechargeClient(rechargeData.AccountId, rechargeData.BundleId, rechargeData.FromAccountId, cancellationToken);
                results.Add(result);
            }
            return results;
        }
        Task<RechargeResult> RechargeClient(Guid toAccountId, Guid bundleId, Guid? fromAccountId, CancellationToken cancellationToken)
        => CreateAndSaveTransaction(
            fromAccountId is null ? GetDefaultRetailAcount() : GetAccount(fromAccountId ?? Guid.Empty),
            GetAccount(toAccountId),
            GetBundle(bundleId), cancellationToken)
          .MapAsync(data => UpdateAccounts(data, cancellationToken))
          .MapAsync(data => CreateAndSaveActiveBundle(data, cancellationToken));

        record TransactionData(AccountTransaction Trasaction, Account From, Account To, Bundle Bundle);
        async Task<TransactionData> CreateAndSaveTransaction(Account from, Account to, Bundle bundle, CancellationToken cancellationToken)
        {
            var transaction = new AccountTransaction()
            {
                Amount = bundle.Amount,
                FromId = from.Id,
                ToId = to.Id,
                BundleId = bundle.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var result = await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new(result.Entity, from, to, bundle);
        }

        async Task<TransactionData> UpdateAccounts(TransactionData data, CancellationToken cancellationToken)
        {
            var from = _dbContext.Accounts.First(x => x.Id == data.From.Id);
            var to = _dbContext.Accounts.First(x => x.Id == data.To.Id);
            data.From.Balance = from.Balance -= data.Bundle.Data;
            data.To.Balance = to.Balance += data.Bundle.Data;
            data.From.UpdatedAt = data.To.UpdatedAt = data.To.UpdatedAt = data.From.UpdatedAt = DateTime.UtcNow;
            _dbContext.Update(from);
            _dbContext.Update(to);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return data;
        }

        async Task<RechargeResult> CreateAndSaveActiveBundle(TransactionData data, CancellationToken cancellationToken)
        {
            var activeBundle = new ActiveBundle()
            {
                AccountTransactionId = data.Trasaction.Id,
                State = ActiveBundleState.Pending,
                CreatedAt = DateTime.UtcNow,
                CurrentBalance = data.Bundle.Data,
                ExpiresAt = DateTime.UtcNow.AddHours(data.Bundle.ExpiresIn),
                OriginalBalance = data.Bundle.Data,
                UpdatedAt = DateTime.UtcNow
            };
            var result = await _dbContext.AddAsync(activeBundle, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new RechargeResult(data.Trasaction, result.Entity);
        }

        Account GetSuperDealerAcount()
        => _dbContext.Users.Include(x => x.Account)
            .Where(x => x.Role == AuthKeys.Roles.SuperAdmin)
            .Select(x => x.Account!)
            .First();
        Account GetDefaultRetailAcount()
       => _dbContext.Accounts.First(x => x.Id == new Guid(AccountKeys.DefaultRetailAccountId));

        Bundle GetBundle(Guid bundleId)
        => _dbContext.Bundles.First(x => x.Id == bundleId);
        Account GetAccount(Guid accountId)
        => _dbContext.Accounts.First(x => x.Id == accountId);
    }
}
