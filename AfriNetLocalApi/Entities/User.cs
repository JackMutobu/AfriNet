using AfriNetLocalApi.Constants;

namespace AfriNetLocalApi.Entities
{
    public class User:IBaseEntity
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = string.Empty;
        public string Fullname => $"{Firstname} {Lastname}";
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = AuthKeys.Roles.Guest;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
