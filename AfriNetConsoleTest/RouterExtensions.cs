using AfriNetConsoleTest.Models;
using HtmlAgilityPack;
using Microsoft.Playwright;

namespace AfriNetConsoleTest
{
    public static class RouterExtensions
    {
        public static TryAsync<string> GetPageTitle(this IPage page)
        => TryAsync(async () =>
        {
            if (page is not null)
            {
                return await page.TitleAsync();
            }
            return string.Empty;
        });

        public static TryAsync<IPage> Login(this IPage page, (string input, string button) selectors, string password)
        => TryAsync(async () =>
        {
            await page.Locator(selectors.input).FillAsync(password);
            await page.ClickAsync(selectors.button);

            var loginContent = await page.ContentAsync();

            if (loginContent.Contains("url=url.replace(\"tplinklogin.net\",\"tplinkwifi.net\");window.location=url"))
                await page.ReloadAsync();

            return page;
        });

        public static TryAsync<IEnumerable<WireLessLiveDevice>> GetWirelessLiveDevices(this IPage page, (string menuFrame, string menu, string subMenu, string mainFrame, string refreshButton, string tableContainer) selectors)
        => page.GoToWirelessBandLiveDevices((selectors.menuFrame, selectors.menu, selectors.subMenu, selectors.mainFrame, selectors.refreshButton))
            .Bind(page => page.GetWireLessLiveDevices((selectors.mainFrame,selectors.tableContainer)));

        private static TryAsync<IPage> GoToWirelessBandLiveDevices(this IPage page, (string menuFrame, string menu, string subMenu, string refreshButtonFrame, string refreshButton) selectors)
        => TryAsync(async () =>
        {
            var menuFrame = await (await page.FrameLocator(selectors.menuFrame).Locator(selectors.menu).ElementHandleAsync()).OwnerFrameAsync();
            await menuFrame!.ClickAsync(selectors.menu);
            await menuFrame.ClickAsync(selectors.subMenu);

            var mainFrame = await (await page.FrameLocator(selectors.refreshButtonFrame).Locator(selectors.refreshButton).ElementHandleAsync()).OwnerFrameAsync();
            await mainFrame!.ClickAsync(selectors.refreshButton);

            await Task.Delay(TimeSpan.FromSeconds(2));//wait for refresh button to execute

            return mainFrame.Page;
        });

        private static TryAsync<IEnumerable<WireLessLiveDevice>> GetWireLessLiveDevices(this IPage page,(string tableFrame, string tableContainer) selectors)
        => TryAsync(async () =>
        {
            var mainFrame = await (await page.FrameLocator(selectors.tableFrame).Locator(selectors.tableContainer).ElementHandleAsync()).OwnerFrameAsync();

            var content = await mainFrame!.ContentAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var nodes = doc.DocumentNode.SelectNodes("//table");
            var table = doc.DocumentNode.SelectNodes("//table").Skip(1).First();
            var devices = new List<WireLessLiveDevice>();
            var cells = table.SelectNodes("//td");

            for (int skip = 0; skip < cells.Count; skip += 6)
            {
                var cellsToProcess = cells.Skip(skip).Take(6).ToList();
                var id = cellsToProcess[0].InnerText;
                var macAddress = cellsToProcess[1].InnerText;
                var currentStatus = cellsToProcess[2].InnerText;
                var receivedPackets = cellsToProcess[3].InnerText;
                var sentPackets = cellsToProcess[4].InnerText;
                var ssid = cellsToProcess[5].InnerText;
                devices.Add(new WireLessLiveDevice(id, macAddress, receivedPackets, sentPackets, ssid) { Status = DeviceStatus.Online});
            }

            return devices.AsEnumerable();
        });
    }
}
