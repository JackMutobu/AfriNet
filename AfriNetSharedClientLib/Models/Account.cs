namespace AfriNetSharedClientLib.Models
{
    public class AccountType
    {
        public const string Dealer = nameof(Dealer);
        public const string Retailer = nameof(Retailer);
        public const string Client = nameof(Client);
    }
    public class Account
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public decimal CurrentBalanceActive { get; set; }
        public IEnumerable<Guid> ActiveBundleIds { get; set; } = Enumerable.Empty<Guid>();
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
