namespace AfriNetLocalApi.Configuration
{
    public class AppSettings
    {
        public string RouterIP { get; init; } = null!;

        public string? RouterUsername { get; init; }

        public string RouterPassword { get; init; } = null!;
    }
}
