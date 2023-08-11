using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;
using AfriNetLocalApi.Services.Auth;
using FastEndpoints.Security;
using System.Security.Claims;

namespace AfriNetLocalApi._Features_.Auth
{
    public record LoginRequest(string Phone, string Password);

    public record LoginResponse(User User, string Token);

    public class Login: Endpoint<LoginRequest,LoginResponse>
    {
        private readonly IAuthService _authService;

        public Login(IAuthService authService)
        {
            _authService = authService;
        }

        public override void Configure()
        {
            Post("/api/auth/login");
            AllowAnonymous();
        }

        public override Task HandleAsync(LoginRequest req, CancellationToken token)
        => _authService.IsValidCrendetials(req.Phone, req.Password, token).ToAsync()
            .MatchAsync(user => SendAsync(new LoginResponse(user, GetToken(user))), error => this.ThrowError(error));

        static string GetToken(User user)
        {
            var jwtToken = JWTBearer.CreateToken(
                    signingKey: AuthKeys.JWTKey,
                    expireAt: DateTime.UtcNow.AddDays(1),
                    priviledges: u =>
                    {
                        u.Roles.Add(user.Role);
                        u.Claims.Add(new(ClaimTypes.MobilePhone,user.Phone));
                        u.Claims.Add(new(ClaimTypes.Email, user.Email ?? string.Empty));
                        u.Claims.Add(new(ClaimTypes.Name, user.Fullname));
                        u.Claims.Add(new(ClaimTypes.Role, user.Role));
                    });
            return jwtToken;
        }
    }
}
