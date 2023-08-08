using AfriNetLocalApi.Constants;

namespace AfriNetLocalApi.Entities
{
    public class User
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = string.Empty;
        public string Fullname => $"{Firstname} {Lastname}";
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = AuthKeys.Roles.Guest;
    }
}
