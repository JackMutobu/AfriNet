namespace AfriNetSharedClientLib.Models
{

    public class Account
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public Guid ActiveBundleId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
