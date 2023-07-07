namespace AfriNetConsoleTest.Models
{
    public enum DeviceStatus
    {
        Online,
        Offline,
        Blocked
    }
    public record Device(string MacAddress)
    {
        public DeviceStatus Status { get; init; } = DeviceStatus.Offline;
    }
}
