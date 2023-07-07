namespace AfriNetConsoleTest;

using Microsoft.Playwright;

public class RouterService
{
    private IPage _page = null!;

    public async Task InitializeAsync()
    {
        using var pw = await Playwright.CreateAsync();
        await using var browser = await pw.Chromium.LaunchAsync();

        _page = await browser.NewPageAsync();
        await _page.GotoAsync("http://192.168.1.78/");
        var title = await _page.TitleAsync();
    }

}
