namespace AfriNetRouterLib;

using AfriNetRouterLib.Models;
using AfriNetRouterLib.Extensions.Version09104;
using AfriNetRouterLib.Interfaces;

public class TpLinkArcherC50Service : IRouterService
{
    private readonly string _password;
    private readonly Uri _homeUrl;
    public TpLinkArcherC50Service(string hostname, string password)
    {
        _password = password;
        _homeUrl = new Uri(hostname);
    }

    private TryAsync<IEnumerable<WireLessLiveDevice>> Get5GConnectedDevices()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetWirelessLiveDevices(("#frame1", "#menu_wl5g", "#menu_wlstat5g", "#frame2", "#refresh", "#tlbHeadMssid")))
        .Map(devices => devices.Map(d => d with { Band = WireLessNetworkBand._5G })));

    private TryAsync<IEnumerable<WireLessLiveDevice>> Get24GConnectedDevices()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetWirelessLiveDevices(("#frame1", "#menu_wl2g", "#menu_wlstat", "#frame2", "#refresh", "#tlbHeadMssid")))
        .Map(devices => devices.Map(d => d with { Band = WireLessNetworkBand._24G })));

    public TryAsync<IEnumerable<WireLessLiveDevice>> GetConnectedDevices()
    => Get5GConnectedDevices().Concat(Get24GConnectedDevices());

    private TryAsync<IEnumerable<MacFilteredWirelessDevice>> Get5GMacFilteringTable()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetMacFilterWireLessDevices(("#frame1", "#menu_wl5g", "#menu_wlacl5g", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead")))
        .Map(devices => devices.Map(d => d with { Band = WireLessNetworkBand._5G })));

    private TryAsync<IEnumerable<MacFilteredWirelessDevice>> Get24GMacFilteringTable()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetMacFilterWireLessDevices(("#frame1", "#menu_wl2g", "#menu_wlacl", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead")))
        .Map(devices => devices.Map(d => d with { Band = WireLessNetworkBand._24G })));

    public TryAsync<IEnumerable<MacFilteredWirelessDevice>> GetBlockedDevices()
    => Get5GMacFilteringTable().Concat(Get24GMacFilteringTable());

    private TryAsync<Unit> Block24GDevice(string macAdress)
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.BlockDeviceInMacFilteringTable(macAdress,
                   ("#frame1", "#menu_wl2g", "#menu_wlacl", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"),
                   ("Add New", "#MAC", "#desc", "#state", "Save")))
        .Map(page => Unit.Default));

    private TryAsync<Unit> Block5GDevice(string macAdress)
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.BlockDeviceInMacFilteringTable(macAdress,
                   ("#frame1", "#menu_wl5g", "#menu_wlacl5g", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"),
                   ("Add New", "#MAC", "#desc", "#state", "Save")))
        .Map(_ => Unit.Default));

    public TryAsync<Unit> BlockDevice(string macAdress)
    => Block24GDevice(macAdress)
       .Bind(_ => Block5GDevice(macAdress));

    private TryAsync<Unit> UnBlock5GDevice(string macAdress)
    => _homeUrl.InitializePlaywrightAsync(page =>
       page.Login(("#pcPassword", "#loginBtn"), _password)
       .Bind(page => page.UnblockDeviceInMacFilteringTable(macAdress,
                  ("#frame1", "#menu_wl5g", "#menu_wlacl5g", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"),
                  "Delete Selected"))
       .Map(_ => Unit.Default));

    private TryAsync<Unit> UnBlock24GDevice(string macAdress)
   => _homeUrl.InitializePlaywrightAsync(page =>
      page.Login(("#pcPassword", "#loginBtn"), _password)
      .Bind(page => page.UnblockDeviceInMacFilteringTable(macAdress,
                 ("#frame1", "#menu_wl2g", "#menu_wlacl", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"),
                 "Delete Selected"))
      .Map(_ => Unit.Default));

    public TryAsync<Unit> UnBlockDevice(string macAdress)
   => UnBlock24GDevice(macAdress)
      .Bind(_ => UnBlock5GDevice(macAdress));
}
