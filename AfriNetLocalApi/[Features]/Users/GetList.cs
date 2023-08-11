using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;

namespace AfriNetLocalApi._Features_.Users
{
    public class GetList:Endpoint<EmptyRequest,IEnumerable<User>>
    {
        private readonly AfriNetLocalDbContext _dbContext;
        public GetList(AfriNetLocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override void Configure()
        {
            Get("/api/users");
            Roles(AuthKeys.Roles.Admin, AuthKeys.Roles.SuperAdmin);
        }

        public async override Task HandleAsync(EmptyRequest req, CancellationToken token)
        {
            var users = await _dbContext.Users.ToListAsync();
            await SendAsync(users);
        }
    }
}
