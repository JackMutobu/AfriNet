using System.Collections.ObjectModel;

namespace AfriNetLocalApi.Entities
{
    public class AccountType
    {
        public const string Dealer = nameof(Dealer);
        public const string Retailer = nameof(Retailer);
        public const string Client = nameof(Client);
    }
    public class Account : IBaseEntity
    {
        public Guid Id { get; set; }

        public decimal Balance { get; set; }
        public string Type { get; set; } = AccountType.Client;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        public static Account Default(string accountType = AccountType.Client) 
            => new() { Balance = 0, Type = accountType,CreatedAt = DateTime.UtcNow };
    }
}
