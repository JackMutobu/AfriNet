using AfriNetLocalApi.Entities;

namespace AfriNetLocalApi._Features_.Clients
{
    public record GetAccountRequest(Guid AccontId);
    public record AccountResponse(Guid Id, decimal Balance, decimal CurrentBalanceActive, string Type, DateTime CreatedAt, DateTime UpdatedAt)
    {
        public IEnumerable<Guid> ActiveBundleIds { get; set; } = Enumerable.Empty<Guid>();
        public DateTime ExpiresAt { get; set; }
    }
    public class GetAccount : Endpoint<GetAccountRequest, AccountResponse>
    {
        private readonly AfriNetLocalDbContext _dbContext;

        public GetAccount(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Configure()
        {
            Get("/api/clients/account");
        }
        record TransactionBundle(Guid TransactionId, Guid ActiveBundleId, string State, DateTime ExpiresAt, decimal OriginalBalance, decimal CurrentBalance, DateTime CreatedAt, Guid BundleId);
        public async override Task HandleAsync(GetAccountRequest req, CancellationToken ct)
        {
            var transactionsQuery = from transaction in _dbContext.Transactions
                                    join activeBundle in _dbContext.ActiveBundles
                                    on transaction.Id equals activeBundle.AccountTransactionId
                                    where transaction.ToId == req.AccontId
                                    where activeBundle.State == ActiveBundleState.Pending || activeBundle.State == ActiveBundleState.Active
                                    select new TransactionBundle
                                    (transaction.Id,
                                    activeBundle.Id,
                                    activeBundle.State,
                                    activeBundle.ExpiresAt,
                                    activeBundle.OriginalBalance,
                                    activeBundle.CurrentBalance,
                                    activeBundle.CreatedAt,
                                    transaction.BundleId);

            var transactions = await transactionsQuery.ToListAsync();

            var account = await _dbContext.Accounts.FirstAsync(x => x.Id == req.AccontId);

            var accountResponse = new AccountResponse(account.Id,
                account.Balance,
                transactions.Sum(x => x.CurrentBalance),
                account.Type, account.CreatedAt,
                account.UpdatedAt);

            if (transactions.Any())
            {
                accountResponse.ActiveBundleIds = transactions.Select(x => x.ActiveBundleId);
                accountResponse.ExpiresAt = transactions.Max(x => x.ExpiresAt);
            }

            await SendAsync(accountResponse);
        }


    }
}
