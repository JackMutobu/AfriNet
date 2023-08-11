namespace AfriNetLocalApi.Entities
{
    public class ActiveBundleState
    {
        public const string Pending = nameof(Pending);
        public const string Active = nameof(Active);
        public const string Expired = nameof(Expired);
        public const string Disabled = nameof(Disabled);
    }
    public class ActiveBundle : IBaseEntity
    {
        public Guid Id { get; set; }

        public decimal CurrentBalance { get; set; }

        public Guid AccountTransactionId { get; set; }
        public AccountTransaction? AccountTransaction { get; set; }

        public string State { get; set; } = ActiveBundleState.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
