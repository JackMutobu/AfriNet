using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;

namespace AfriNetLocalApi.Data
{
    public class AfriNetLocalDbContext : DbContext
    {
        public AfriNetLocalDbContext(DbContextOptions<AfriNetLocalDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<AccountTransaction> Transactions { get; set; } = null!;
        public DbSet<Bundle> Bundles { get; set; } = null!;
        public DbSet<ActiveBundle> ActiveBundles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                   .HasIndex(p => p.Phone)
                   .IsUnique();

            var accountId = "2301D884-221A-4E7D-B509-0113DCC043E1";
            var userId = "198E6AEA-6827-4562-B415-242146DE9B9B";
            var defaultRetailerId = "e860d79d-18e5-4d70-93e6-6a09a21dc6ff";
            var defaultRetailerAccountId = "d3d0a5cb-653c-496d-a188-2eaef509dfee";

            builder.Entity<Account>().HasData(new Account()
            {
                Balance = 0,
                CreatedAt = DateTime.UtcNow,
                Id = new Guid(accountId),
                Type = AccountType.Dealer,
                UpdatedAt = DateTime.UtcNow
            });
            builder.Entity<User>().HasData(new User() 
            {
                Id= new Guid(userId),
                Firstname = "Jack",
                Lastname = "Mutobu Nzanzu",
                Phone = "0997186014",
                Role = AuthKeys.Roles.SuperAdmin,
                PasswordHash = "Test@243",
                AccountId = new Guid(accountId)
            });
            builder.Entity<Account>().HasData(new Account()
            {
                Balance = 0,
                CreatedAt = DateTime.UtcNow,
                Id = new Guid(defaultRetailerAccountId),
                Type = AccountType.Retailer,
                UpdatedAt = DateTime.UtcNow
            });
            builder.Entity<User>().HasData(new User()
            {
                Id = new Guid(defaultRetailerId),
                Firstname = "Retailer",
                Lastname = "Default",
                AccountId = new Guid(defaultRetailerAccountId),
                CreatedAt = DateTime.UtcNow,
                PasswordHash = "Test@243",
                Role = AuthKeys.Roles.Retailer,
                Phone = "0997186015"
            });
        }
    }
}
