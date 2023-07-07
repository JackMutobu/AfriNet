namespace AfriNetConsoleTest.Models
{
    public record WireLessLiveDevice(string Id, string MacAddress, string ReceivedPackets, string SentPackets, string SSID) : Device(MacAddress);
}
