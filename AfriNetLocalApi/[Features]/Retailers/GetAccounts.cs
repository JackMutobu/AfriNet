using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;

namespace AfriNetLocalApi._Features_.Retailers
{
    public record GetRequest(int Skip = 0, int Take = 10);
    public class GetAccounts: Endpoint<GetRequest, IEnumerable<Account>>
    {
        private readonly AfriNetLocalDbContext _dbContext;
        public GetAccounts(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Configure()
        {
            Get("/api/retailers/accounts");
            Roles(AuthKeys.Roles.SuperAdmin);
        }

        public override Task HandleAsync(GetRequest req, CancellationToken ct)
        => _dbContext.Accounts
            .Where(x => x.Type == AccountType.Retailer)
            .Skip(req.Skip).Take(req.Take).ToListAsync()
            .Map(accounts => SendAsync(accounts));
    }
}
