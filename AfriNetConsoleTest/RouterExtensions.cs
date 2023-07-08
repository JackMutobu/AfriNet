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
            .Bind(page => page.GetWireLessLiveDevices((selectors.mainFrame, selectors.tableContainer)));

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

        private static TryAsync<IEnumerable<WireLessLiveDevice>> GetWireLessLiveDevices(this IPage page, (string tableFrame, string tableContainer) selectors)
        => TryAsync(async () =>
        {
            var mainFrame = await (await page.FrameLocator(selectors.tableFrame).Locator(selectors.tableContainer).ElementHandleAsync()).OwnerFrameAsync();

            var content = await mainFrame!.ContentAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(content);
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
                devices.Add(new WireLessLiveDevice(id, macAddress, receivedPackets, sentPackets, ssid) { Status = DeviceStatus.Online });
            }

            return devices.AsEnumerable();
        });

        public static TryAsync<IEnumerable<MacFilteredWirelessDevice>> GetMacFilterWireLessDevices(this IPage page, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton, string tableContainer) selectors)
         => page.GoToWirelessBandMacFiltering((selectors.menuFrame, selectors.menu, selectors.subMenu, selectors.macFilteringPageSampleElement, selectors.macFilteringEnabledSpan, selectors.enableButtonFrame, selectors.enableButton))
            .Bind(page => page.GetMacFilterWireLessDevicesFromTable((selectors.enableButtonFrame, selectors.tableContainer)));

        private static TryAsync<IPage> GoToWirelessBandMacFiltering(this IPage page, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton) selectors)
        => TryAsync(async () =>
        {
            var mainFrame = await GotToFilteringPage(page, selectors);
            mainFrame = await EnableFiltering(mainFrame!, page, selectors);

            return mainFrame!.Page;

            static async Task<bool> IsFilteringEnabled((string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton) selectors, IFrame? mainFrame)
            {
                var enabledLabel = mainFrame!.Locator(selectors.macFilteringEnabledSpan);
                var enabledLabelAttributes = await enabledLabel.GetAttributeAsync("class");
                return !enabledLabelAttributes!.Contains("nd");
            }

            static async Task<IFrame?> EnableFiltering(IFrame mainFrame, IPage page, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton) selectors)
            {
                var isEnabled = await IsFilteringEnabled(selectors, mainFrame);

                if (!isEnabled)
                {
                    await mainFrame!.ClickAsync(selectors.enableButton);
                    await Task.Delay(TimeSpan.FromSeconds(15));
                    await page.ReloadAsync();
                    var currentFrame = await GotToFilteringPage(page, selectors);
                    await EnableFiltering(currentFrame!, page, selectors);
                }

                return mainFrame;
            }
        });

        private static async Task<IFrame?> GetMenuFrame(IPage page, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton) selectors)
        {
            return await (await page.FrameLocator(selectors.menuFrame).Locator(selectors.menu).ElementHandleAsync()).OwnerFrameAsync();
        }

        private static async Task<IFrame?> GetMainFrame(IPage page, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton) selectors)
        {
            return await (await page.FrameLocator(selectors.enableButtonFrame).Locator(selectors.macFilteringPageSampleElement).ElementHandleAsync()).OwnerFrameAsync();
        }

        private static async Task<IFrame?> GotToFilteringPage(IPage page, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton) selectors)
        {
            var menuFrame = await GetMenuFrame(page, selectors);
            await menuFrame!.ClickAsync(selectors.menu);
            await menuFrame.ClickAsync(selectors.subMenu);
            var mainFrame = await GetMainFrame(page, selectors);
            return mainFrame;
        }

        private static TryAsync<IEnumerable<MacFilteredWirelessDevice>> GetMacFilterWireLessDevicesFromTable(this IPage page, (string tableFrame, string tableContainer) selectors)
        => TryAsync(async () =>
        {
            var mainFrame = await (await page.FrameLocator(selectors.tableFrame).Locator(selectors.tableContainer).ElementHandleAsync()).OwnerFrameAsync();

            var content = await mainFrame!.ContentAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var table = doc.DocumentNode.SelectNodes("//table").First();
            var devices = new List<MacFilteredWirelessDevice>();
            var cells = table.SelectNodes("//td");

            for (int skip = 0; skip < cells.Count; skip += 6)
            {
                var cellsToProcess = cells.Skip(skip).Take(6).ToList();
                var macAddress = cellsToProcess[1].InnerText;
                var currentStatus = cellsToProcess[2].InnerText;
                var host = cellsToProcess[3].InnerText;
                var description = cellsToProcess[4].InnerText;
                devices.Add(new MacFilteredWirelessDevice(macAddress, currentStatus, host, description));
            }

            return devices.AsEnumerable();
        });

        public static TryAsync<IPage> BlockDeviceInMacFilteringTable(this IPage page, string macAdress, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton, string tableContainer) selectors, (string addNewValue, string macInput, string descriptionInput, string stateSelect, string saveButtonValue) formSelectors)
         => page.GoToWirelessBandMacFiltering((selectors.menuFrame, selectors.menu, selectors.subMenu, selectors.macFilteringPageSampleElement, selectors.macFilteringEnabledSpan, selectors.enableButtonFrame, selectors.enableButton))
            .Bind(page => page.AddDeviceToMacFilteringTable(macAdress,(selectors.menuFrame, selectors.menu, selectors.subMenu, selectors.macFilteringPageSampleElement, selectors.macFilteringEnabledSpan, selectors.enableButtonFrame, selectors.enableButton), formSelectors));

        private static TryAsync<IPage> AddDeviceToMacFilteringTable(this IPage page, string macAdress, (string menuFrame, string menu, string subMenu, string macFilteringPageSampleElement, string macFilteringEnabledSpan, string enableButtonFrame, string enableButton) filterPageselectors, (string addNewValue, string macInput, string descriptionInput, string stateSelect, string saveButtonValue) formSelectors)
        => TryAsync(async () =>
        {
            var mainFrame = await GotToFilteringPage(page, filterPageselectors);
            await mainFrame!.GetByText("Add New").ClickAsync();

            filterPageselectors.macFilteringPageSampleElement = "#con";
            mainFrame = await GetMainFrame(page, filterPageselectors);

            await mainFrame!.Locator(formSelectors.macInput).FillAsync(macAdress);

            await mainFrame!.Locator(formSelectors.descriptionInput).FillAsync("Blocked");

            await mainFrame!.Locator(formSelectors.stateSelect).SelectOptionAsync("Enabled");

            await mainFrame!.GetByText("Save").ClickAsync();

            await Task.Delay(300);

            return page;
        });
    }
}
