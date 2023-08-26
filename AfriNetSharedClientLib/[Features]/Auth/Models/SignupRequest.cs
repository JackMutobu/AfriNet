namespace AfriNetSharedClientLib.Auth.Models
{
    public class SignupUserRequest
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = "client";
    }
}
