namespace AfriNetLocalApi.Entities
{
    public class AccountTransaction : IBaseEntity
    {
        public Guid Id { get; set; }

        public Guid FromId { get; set; }
        public Account? From { get; set; }

        public Guid ToId { get; set; }
        public Account? To { get; set; }

        public decimal Amount { get; set; }

        public Guid BundleId { get; set; }
        public Bundle? Bundle { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
