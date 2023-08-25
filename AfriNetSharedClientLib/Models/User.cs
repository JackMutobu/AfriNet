namespace AfriNetSharedClientLib.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = string.Empty;
        public string Fullname => $"{Firstname} {Lastname}";
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = null!;
        public Guid AccountId { get; set; }
    }
}
