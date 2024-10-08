﻿using System.Collections.ObjectModel;

namespace AfriNetLocalApi.Entities
{
    public class BundleType
    {
        public const string Daily = nameof(Daily);
        public const string Weekly = nameof(Weekly);
        public const string Monthly = nameof(Monthly);
        public const string Unlimited = nameof(Unlimited);
    }
    public class BundleFor
    {
        public const string Retailer = nameof(Retailer);
        public const string Dealer = nameof(Dealer);
        public const string Client = nameof(Client);
    }
    public class Bundle:IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        //In Local Currency
        public decimal Amount { get; set; }

        //In MB
        public decimal Data { get; set; }

        //In Hours
        public int ExpiresIn { get; set; }

        public bool IsUnlimited { get; set; }

        public string For { get; set; } = BundleFor.Client;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        public ICollection<AccountTransaction> Transactions { get; set; } = new Collection<AccountTransaction>();
    }
}
