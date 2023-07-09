namespace AfriNetRouterLib.Models
{
    public class MacFilteringStatus
    {
        public const string Enabled = nameof(Enabled);

        public const string Disabled = nameof(Disabled);
    }
    public record MacFilteredWirelessDevice(string MacAddress, string FilteringStatus, string Host, string Description) : Device(MacAddress) 
    {
        public string Band { get; init; } = WireLessNetworkBand.Unknown;

        public DateTime CurrentTime { get; init; } = DateTime.UtcNow;
    };
}
