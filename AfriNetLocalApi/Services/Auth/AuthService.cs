using AfriNetLocalApi.Entities;
using LanguageExt.Common;

namespace AfriNetLocalApi.Services.Auth
{
    public interface IAuthService
    {
        Task<Either<Error, User>> IsValidCrendetials(string username, string password, CancellationToken token);
        Task<User> CreateUser(User user, string password, CancellationToken cancellationToken);
        Task<User> CreateUser(User user, string password, Guid accountId, CancellationToken cancellationToken);
    }

    public class AuthService : IAuthService
    {
        private readonly AfriNetLocalDbContext _dbContext;
        public AuthService(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Either<Error, User>> IsValidCrendetials(string username, string password, CancellationToken token)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Phone == username, token);
            if (user is not null && user.PasswordHash == password)
                return user;
            return Error.New("Invalid credentials");
        }

        public async Task<User> CreateUser(User user, string password, CancellationToken cancellationToken)
        {
            user.PasswordHash = password;
            var result = await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public Task<User> CreateUser(User user, string password, Guid accountId, CancellationToken cancellationToken)
        {
            user.AccountId = accountId;
            return CreateUser(user, password, cancellationToken);
        }
    }
}
