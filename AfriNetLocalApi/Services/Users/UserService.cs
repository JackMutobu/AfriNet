using AfriNetLocalApi.Entities;
using LanguageExt.Common;

namespace AfriNetLocalApi.Services.Users
{
    public interface IUserService
    {
        Task<List<User>> GetUsers(int skip, int take, CancellationToken cancellationToken);
        Task<Either<User, Error>> Update(User user, CancellationToken cancellationToken);
    }

    public class UserService : IUserService
    {
        private readonly AfriNetLocalDbContext _dbContext;

        public UserService(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<User>> GetUsers(int skip, int take, CancellationToken cancellationToken)
            => _dbContext.Users.Skip(skip).Take(take).ToListAsync(cancellationToken);

        public async Task<Either<User, Error>> Update(User user, CancellationToken cancellationToken)
        {
            var savedUser = _dbContext.Users.FirstOrDefault();
            if (savedUser is null)
                return Error.New("User not found");

            savedUser.AccountId = user.AccountId;
            savedUser.Email = user.Email;
            savedUser.Firstname = user.Firstname;
            savedUser.Lastname = user.Lastname;
            savedUser.UpdatedAt = DateTime.UtcNow;

            _dbContext.Users.Update(savedUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return savedUser;
        }
    }
}
