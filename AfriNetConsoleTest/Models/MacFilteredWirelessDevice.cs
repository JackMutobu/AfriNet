namespace AfriNetConsoleTest.Models
{
    public class MacFilteringStatus
    {
        public const string Enabled = nameof(Enabled);

        public const string Disabled = nameof(Disabled);
    }
    public record MacFilteredWirelessDevice : WireLessLiveDevice
    {
        //If Enabled it means device is not allowed on the network
        public MacFilteredWirelessDevice(string Id, string MacAddress, string ReceivedPackets, string SentPackets, string SSID) : base(Id, MacAddress, ReceivedPackets, SentPackets, SSID)
        {
        }

        public MacFilteredWirelessDevice(WireLessLiveDevice wireLessLiveDevice) : base(wireLessLiveDevice) { }

        public MacFilteredWirelessDevice(string macAddress, string filteringStatus, string host, string description): base(string.Empty, macAddress, string.Empty, string.Empty, host)
        {
            FilteringStatus = filteringStatus; 
            Description = description;
        }

        public string Host => SSID;

        public string FilteringStatus { get; init; } = MacFilteringStatus.Disabled;

        public string? Description { get; init; }
    }
}
