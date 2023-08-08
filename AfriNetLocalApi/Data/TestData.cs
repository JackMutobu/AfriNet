using AfriNetLocalApi.Entities;

namespace AfriNetLocalApi.Data
{
    public static class TestData
    {
        public static Task<List<User>> GetUsersAsync()
            => Task.FromResult(new List<User>()
            {
                new User{Firstname ="Jack",Lastname="Mutobu", Phone = "0997186014", PasswordHash="Test@243"}
            });
    }
}
