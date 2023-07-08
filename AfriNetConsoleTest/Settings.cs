namespace AfriNetConsoleTest
{
    public class Settings
    {
        public Settings()
        {
            RouterIP = "192.168.1.78";
            RouterUsername = "admin";
            RouterPassword = "admin@123";
        }

        public string RouterIP { get; init; } = null!;

        public string RouterUsername { get; init; } = null!;

        public string RouterPassword { get; init; } = null!;
    }
}
