namespace AfriNetRouterLib.Models
{
    public class WireLessNetworkBand
    {
        public const string _5G = "5G";
        public const string _24G = "2.4G";
        public const string Unknown = nameof(Unknown);
    }
    public record WireLessLiveDevice(string Id, string MacAddress, string ReceivedPackets, string SentPackets, string SSID) : Device(MacAddress)
    {
        public string Band { get; init; } = WireLessNetworkBand.Unknown;

        public DateTime CurrentTime { get; init; } = DateTime.UtcNow;
    }
}
