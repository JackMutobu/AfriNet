namespace AfriNetLocalApi.Constants
{
    public class AuthKeys
    {
        public const string JWTKey = "TokenSigningKey5";

        public static class Roles
        {
            public const string SuperAdmin = "superadmin";
            public const string Admin = "admin";
            public const string Client = "client";
            public const string Retailer = "retailer";
            public const string Guest = "guest";
        }
    }
}
