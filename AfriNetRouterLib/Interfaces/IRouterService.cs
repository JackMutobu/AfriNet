using AfriNetRouterLib.Models;

namespace AfriNetRouterLib.Interfaces
{
    public interface IRouterService
    {
        TryAsync<Unit> BlockDevice(string macAdress);
        TryAsync<IEnumerable<MacFilteredWirelessDevice>> GetBlockedDevices();
        TryAsync<IEnumerable<WireLessLiveDevice>> GetConnectedDevices();
        TryAsync<Unit> UnBlockDevice(string macAdress);
    }
}