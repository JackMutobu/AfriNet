using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;
using AfriNetLocalApi.Services.Accounts;
using AfriNetLocalApi.Services.Auth;

namespace AfriNetLocalApi._Features_.Auth
{
    public record SingupRequest(User User, string Password);

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
           .MapAsync(account => _authService.CreateUser(req.User, req.Password,account.Id, token))
           .Map(user => SendAsync(new SingupResponse(user)));

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
