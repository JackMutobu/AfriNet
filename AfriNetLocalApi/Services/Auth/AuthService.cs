using AfriNetLocalApi.Data;
using AfriNetLocalApi.Entities;
using LanguageExt.Common;

namespace AfriNetLocalApi.Services.Auth
{
    public interface IAuthService
    {
        Task<Either<Error, User>> IsValidCrendetials(string username, string password, CancellationToken token);
    }

    public class AuthService : IAuthService
    {
        public async Task<Either<Error, User>> IsValidCrendetials(string username, string password, CancellationToken token)
        {
            var users = await TestData.GetUsersAsync();
            var user =  users.FirstOrDefault(x => x.Phone == username);
            if (user is not null && user.PasswordHash == password)
                return user;
            return Error.New("Invalid credentials");
        }
    }
}
