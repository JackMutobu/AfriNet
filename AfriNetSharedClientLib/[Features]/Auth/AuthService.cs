using AfriNetSharedClientLib.Auth.Services;
using AfriNetSharedClientLib.Auth.Models;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using AfriNetSharedClientLib.Models;

namespace AfriNetSharedClientLib._Features_.Auth
{
    public record LoginRequest(string Phone, string Password);
    public record SignupRequest(SignupUserRequest User, string Password);
    public record Token(string Value);
    public class AuthService : IAuthService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public AuthService(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public async Task<Option<CurrentUser>> GetCurrentUser(CancellationToken token)
        {
            var user = await _localStorage.GetItemAsync<CurrentUser?>(AuthKeys.LocalUser, token);
            return user is null ? None : user;
        }

        public async Task<Unit> SetCurrentUser(CurrentUser user, CancellationToken token)
        {
            await _localStorage.SetItemAsync(AuthKeys.LocalUser,user, token);
            return Unit.Default;
        }

        record LoginResponse(User User, string Token);
        public TryAsync<string> Login(LoginRequest request, string returnUrl, CancellationToken cancellationToken)
        => TryAsync(
            _httpClient.PostAsJsonAsync("auth/login", request, cancellationToken)
            .Bind(result => result.ToHttpResult())
            .MapAsync(response => response.Deserialize<LoginResponse>())
            .MapAsync(result => SaveToLocalStorage(result!.User,result.Token))
            .Map(result => result.token));

        record SignupResponse(User User);

        public TryAsync<User> Signup(SignupRequest request, CancellationToken cancellationToken)
        => TryAsync(
            _httpClient.PostAsJsonAsync("auth/signup", request, cancellationToken)
            .Bind(result => result.ToHttpResult())
            .MapAsync(response => response.Deserialize<SignupResponse>())
            .Map(result => result!.User));

        private async Task<(CurrentUser user, string token)> SaveToLocalStorage(User user, string token)
        {
            await _localStorage.SetItemAsStringAsync(AuthKeys.Token, token);
            var currentUser = new CurrentUser()
            {
                AccessToken = token,
                Phone = user.Phone,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                IsAuthenticated = true,
                Role = user.Role,
                Id = user.Id,
                AccountId = user.AccountId
            };
            await _localStorage.SetItemAsync(AuthKeys.LocalUser, currentUser);
            return (currentUser,token);
        }

        public TryAsync<Unit> Logout(CancellationToken token)
        => TryAsync(Unit.Default);

        public async Task<string> GetToken(CancellationToken token)
        => await _localStorage.GetItemAsStringAsync(AuthKeys.Token, token);
    }
}
