namespace AfriNetSharedClientLib.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Data { get; set; }
        public string Retailer { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
