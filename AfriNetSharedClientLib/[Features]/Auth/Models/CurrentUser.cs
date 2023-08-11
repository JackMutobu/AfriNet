using AfriNetSharedClientLib.Models;

namespace AfriNetSharedClientLib.Auth.Models
{
    public class CurrentUser:User
    {
        public bool IsAuthenticated { get; init; }

        public string? AccessToken { get; set; }

        public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
    }
}
