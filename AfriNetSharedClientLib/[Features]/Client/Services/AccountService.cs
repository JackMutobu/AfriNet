using AfriNetSharedClientLib.Auth.Services;
using LanguageExt.Pipes;
using System.Net.Http.Json;
using System.Threading;

namespace AfriNetSharedClientLib.Accounts.Services
{
    public interface IAccountService
    {
        TryAsync<Account> GetAccount(Guid accountId, CancellationToken token);
        TryAsync<IEnumerable<Transaction>> GetTransactions(Guid accountId, int skip, int take, CancellationToken token);
        TryAsync<RechargeResponse> Recharge(Guid clientId, Guid bundleId, int quantity, CancellationToken token);
    }
    public record RechargeResponse(IEnumerable<Guid> TransactionIds, IEnumerable<Guid> ActiveBundleIds);
    public partial class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public AccountService(IAuthService authService, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public TryAsync<Account> GetAccount(Guid accountId, CancellationToken token)
        => TryAsync(
            _authService.SetAuthToken(_httpClient, token)
            .MapAsync(client => client.GetFromJsonAsync<Account>($"clients/account?AccontId={accountId}", token))
            .Map(account => account ?? throw new Exception("Account can not be null")));
        public TryAsync<IEnumerable<Transaction>> GetTransactions(Guid accountId, int skip, int take, CancellationToken token)
        => TryAsync(
            _authService.SetAuthToken(_httpClient, token)
            .MapAsync(client => client.GetFromJsonAsync<IEnumerable<Transaction>>($"clients/transactions?ClientId={accountId}&Skip={skip}&Take={take}", token))
            .Map(transactions => transactions ?? Enumerable.Empty<Transaction>()));
        record RechargeRequest(Guid ClientId, Guid BundleId, int Quantity);
        public TryAsync<RechargeResponse> Recharge(Guid clientId, Guid bundleId, int quantity, CancellationToken token)
        => TryAsync(
            _authService.SetAuthToken(_httpClient, token)
            .MapAsync(client => client.PostAsJsonAsync("recharges/client", new RechargeRequest(clientId, bundleId, quantity), token))
            .Bind(result => result.ToHttpResult())
            .MapAsync(response => response.Deserialize<RechargeResponse>())
            .Map(result => result ?? throw new Exception("Recharge failed"))
            );
    }
}
