using AfriNetSharedClientLib._Features_.Auth;
using AfriNetSharedClientLib.Auth.Models;
using AfriNetSharedClientLib.Models;

namespace AfriNetSharedClientLib.Auth.Services
{
    public interface IAuthService
    {
        TryAsync<User> Signup(SignupRequest request, CancellationToken cancellationToken);
        TryAsync<string> Login(LoginRequest request, string returnUrl, CancellationToken token);
        TryAsync<Unit> Logout(CancellationToken token);
        Task<Option<CurrentUser>> GetCurrentUser(CancellationToken token);
        Task<Unit> SetCurrentUser(CurrentUser user, CancellationToken token);
        Task<string> GetToken(CancellationToken token);
    }
}