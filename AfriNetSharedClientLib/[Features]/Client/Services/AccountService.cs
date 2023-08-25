using AfriNetSharedClientLib.Auth.Services;
using AfriNetSharedClientLib.Models;
using AfriNetSharedClientLib.Models.Requests;
using System.Net.Http.Json;

namespace AfriNetSharedClientLib.Accounts.Services
{
    public interface IAccountService
    {
        TryAsync<Account> GetAccount(Guid accountId, CancellationToken token);
    }

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
            .MapAsync(client => client.GetFromJsonAsync<Account>($"clients/account?AccountId={accountId}", token))
            .Map(account => account ?? throw new Exception("Account can not be null")));
    }
}
