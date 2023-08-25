using AfriNetCustomerApp.Services;
using AfriNetSharedClientLib;
using AfriNetSharedClientLib._Features_.Auth;
using AfriNetSharedClientLib.Auth.Services;
using AfriNetSharedClientLib.Common.Bundles;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace AfriNetCustomerApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IBundleService, BundleService>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
#if WINDOWS
            var baseAdress = "https://localhost:44397/api/";
#else   
            var baseAdress = "https://192.168.1.71:45455/api/";
#endif
            builder.Services.AddScoped(_ =>
            {
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                return new HttpClient(handler) { BaseAddress = new Uri(baseAdress) };
            });
            builder.Services.AddScoped<HostAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<HostAuthenticationStateProvider>());
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
		    builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}