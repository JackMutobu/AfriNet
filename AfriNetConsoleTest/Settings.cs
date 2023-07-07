namespace AfriNetConsoleTest
{
    public class Settings
    {
        public static Settings Default { get; } = new Settings()
        {
            RouterHost = "192.168.1.78",
            RouterUsername = "admin",
            RouterPassword = "admin@123",
        };

        public string RouterHost { get; init; } = null!;

        public string RouterUsername { get; init; } = null!;

        public string RouterPassword { get; init; } = null!;
    }
}
