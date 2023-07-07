using LanguageExt;
using Microsoft.Playwright;

namespace AfriNetConsoleTest
{
    public static class PlaywrightExtensions
    {
        public static async Task<T> InitializePlaywrightAsync<T>(this string initialUrl, Func<IPage,Task<T>> action)
        {
            using var pw = await Playwright.CreateAsync();
            await using var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync(initialUrl);
            return await action(page);
        }

        public static TryAsync<T> InitializePlaywrightAsync<T>(this Uri initialUrl, Func<IPage, T> action)
        => TryAsync(async () =>
        {
            using var pw = await Playwright.CreateAsync();
            await using var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync(initialUrl.ToString());
            return action(page);
        });

        public static TryAsync<T> InitializePlaywrightAsync<T>(this Uri initialUrl, Func<IPage, TryAsync<T>> action)
        => TryAsync(async () =>
        {
            using var pw = await Playwright.CreateAsync();
            await using var browser = await pw.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync(initialUrl.ToString());
            var result = default(T);
            await action(page).Match<T>(success => result = success, exception => throw exception);

            return result!;
        });
    }
}
