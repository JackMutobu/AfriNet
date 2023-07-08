namespace AfriNetRouterLib.Models
{
    public class LiveDeviceStatus
    {
        public const string Associated = nameof(Associated);

        public const string Unknown = nameof(Unknown);
    } 
    public record Device(string MacAddress)
    {
        public string LiveStatus { get; init; } = LiveDeviceStatus.Unknown;
    }
}
