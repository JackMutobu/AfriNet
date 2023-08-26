using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;
using AfriNetLocalApi.Services.Accounts;
using AfriNetLocalApi.Services.Auth;

namespace AfriNetLocalApi._Features_.Auth
{
    public class UserRequest
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = AuthKeys.Roles.Guest;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
    }
    public record SingupRequest(UserRequest User, string Password);
    public record SingupResponse(User User);
    public class Signup: Endpoint<SingupRequest, SingupResponse>
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;

        public Signup(IAuthService authService, IAccountService accountService)
        {
            _authService = authService;
            _accountService = accountService;
        }

        public override void Configure()
        {
            Post("/api/auth/signup");
            AllowAnonymous();
        }

        public override Task HandleAsync(SingupRequest req, CancellationToken token)
       => _accountService.Create(Account.Default(GetAccountType(req.User.Role)), token)
           .MapAsync(account => _authService.CreateUser(req.User.Adapt<User>(), req.Password,account.Id, token))
           .Map(user => SendAsync(new SingupResponse(user), 201));

        static string GetAccountType(string userRole)
            => userRole switch
            {
                AuthKeys.Roles.Admin => AccountType.Dealer,
                AuthKeys.Roles.SuperAdmin => AccountType.Dealer,
                AuthKeys.Roles.Retailer => AccountType.Retailer,
                _ => AccountType.Client
            };
    }
}
