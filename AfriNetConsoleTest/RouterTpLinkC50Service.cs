namespace AfriNetConsoleTest;

using AfriNetConsoleTest.Models;
using Microsoft.Playwright;

public class RouterTpLinkC50Service
{
    private readonly string _password;
    private readonly Uri _homeUrl;
    public RouterTpLinkC50Service(string hostname, string password)
    {
        _password = password;
        _homeUrl = new Uri(hostname);
    }

    public TryAsync<IEnumerable<WireLessLiveDevice>> Get5GConnectedDevices()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetWirelessLiveDevices(("#frame1", "#menu_wl5g", "#menu_wlstat5g", "#frame2", "#refresh", "#tlbHeadMssid"))));

    public TryAsync<IEnumerable<WireLessLiveDevice>> Get24GConnectedDevices()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetWirelessLiveDevices(("#frame1", "#menu_wl2g", "#menu_wlstat", "#frame2", "#refresh", "#tlbHeadMssid"))));

    public TryAsync<IEnumerable<MacFilteredWirelessDevice>> Get5GMacFilteringTable()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetMacFilterWireLessDevices(("#frame1", "#menu_wl5g", "#menu_wlacl5g", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"))));

    public TryAsync<IEnumerable<MacFilteredWirelessDevice>> Get24GMacFilteringTable()
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.GetMacFilterWireLessDevices(("#frame1", "#menu_wl2g", "#menu_wlacl", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"))));

    public TryAsync<Unit> Block24GDevice(string macAdress)
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.BlockDeviceInMacFilteringTable(macAdress,
                   ("#frame1", "#menu_wl2g", "#menu_wlacl", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"),
                   ("Add New","#MAC", "#desc", "#state", "Save")))
        .Map(page => Unit.Default));

    public TryAsync<Unit> Block5GDevice(string macAdress)
    => _homeUrl.InitializePlaywrightAsync(page =>
        page.Login(("#pcPassword", "#loginBtn"), _password)
        .Bind(page => page.BlockDeviceInMacFilteringTable(macAdress,
                   ("#frame1", "#menu_wl5g", "#menu_wlacl5g", "#t_rule", "#acl_enabled", "#frame2", "#acl_en", ".thead"),
                   ("Add New", "#MAC", "#desc", "#state", "Save")))
        .Map(_ => Unit.Default));

    public TryAsync<Unit> BlockDevice(string macAdress)
    => Block24GDevice(macAdress)
       .Bind(_ => Block5GDevice(macAdress));
}
