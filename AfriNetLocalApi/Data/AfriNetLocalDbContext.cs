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
        }
    }
}
