using AfriNetLocalApi.Constants;

namespace AfriNetLocalApi._Features_.Clients
{
    public record TransactionResponse(Guid Id, decimal Amount, decimal Data, string Retailer, DateTime Date);
    public record GetClientTransactionRequest(Guid ClientId, int Skip = 0, int Take = 10);
    public class GetTransactions:Endpoint<GetClientTransactionRequest, IEnumerable<TransactionResponse>>
    {
        private readonly AfriNetLocalDbContext _dbContext;
        public GetTransactions(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Configure()
        {
            Get("/api/clients/transactions");
            Roles(AuthKeys.Roles.Client, AuthKeys.Roles.Admin, AuthKeys.Roles.SuperAdmin);
        }

        public override Task HandleAsync(GetClientTransactionRequest req, CancellationToken ct)
        => _dbContext.Transactions
            .Include(x => x.Bundle)
            .Include(x => x.From)
            .Where(x => x.ToId == req.ClientId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(req.Skip)
            .Take(req.Take)
            .ToListAsync()
            .Map(transactions => transactions.Select(x => new TransactionResponse(
                x.Id, 
                x.Amount,
                x.Bundle!.Data,
                x.FromId == new Guid(AccountKeys.DefaultRetailAccountId) ? AccountKeys.Company : x.From!.Id.ToString(),
                x.CreatedAt)))
            .Map(result => SendAsync(result));
    }
}
