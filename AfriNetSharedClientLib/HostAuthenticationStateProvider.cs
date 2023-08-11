using AfriNetSharedClientLib.Auth.Services;
using AfriNetSharedClientLib.Auth.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using AfriNetSharedClientLib._Features_.Auth;

namespace AfriNetSharedClientLib
{
    public class HostAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _authService;
        private CurrentUser? _currentUser;

        public HostAuthenticationStateProvider(IAuthService authService)
        {
            _authService = authService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authState = await GetAuthenticationState(CancellationToken.None)
                .Match(state => state, () => new AuthenticationState(new ClaimsPrincipal()));
            return authState!;
        }

        private OptionAsync<AuthenticationState> GetAuthenticationState(CancellationToken token)
        => GetCurrentUser(token)
            .Where(user => user.IsAuthenticated)
            .Map(GetClaims)
            .Map(claims => new ClaimsIdentity(claims, nameof(HostAuthenticationStateProvider)))
            .Map(identity => new AuthenticationState(new ClaimsPrincipal(identity)));

        private IEnumerable<Claim> GetClaims(CurrentUser userInfo)
            => new[] {
                        new Claim(ClaimTypes.Name, userInfo.Fullname),
                        new Claim(ClaimTypes.Email,userInfo.Email ?? string.Empty),
                        new Claim(ClaimTypes.Role,userInfo.Role),
                        new Claim(ClaimTypes.MobilePhone,userInfo.Phone),
                        new Claim(ClaimTypes.NameIdentifier,userInfo.Fullname)
                    }
                    .Concat(userInfo.Claims.Select(c => new Claim(c.Key, c.Value)));

        private OptionAsync<CurrentUser> GetCurrentUser(CancellationToken token)
            => _currentUser is not null && _currentUser.IsAuthenticated
                ? _currentUser
                : _authService.GetCurrentUser(token).ToAsync();

        public TryAsync<Unit> Logout(CancellationToken token)
        => _authService.Logout(token);

        public TryOptionAsync<string> Login(LoginRequest loginRequest, string returnUrl, CancellationToken token)
        => _authService.Login(loginRequest, returnUrl, token).ToTryOption();

        public void UpdateAuthenticateState()
        => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
