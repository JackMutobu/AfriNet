using AfriNetSharedClientLib.Auth.Services;
using AfriNetSharedClientLib.Models.Requests;
using System.Net.Http.Json;

namespace AfriNetSharedClientLib.Common.Bundles
{
    public interface IBundleService
    {
        TryAsync<IEnumerable<Bundle>> GetBundles(PaginationRequest request, CancellationToken token);
        string GetBundleSize(string type, decimal data);
        string GetBundleValidity(string type, int expiresInHours);
    }

    public class BundleService : IBundleService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public BundleService(IAuthService authService,HttpClient httpClient)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public TryAsync<IEnumerable<Bundle>> GetBundles(PaginationRequest request, CancellationToken token)
        => TryAsync(
            _authService.SetAuthToken(_httpClient, token)
            .MapAsync(client => client.GetFromJsonAsync<IEnumerable<Bundle>>($"bundles?Skip={request.Skip}&Take={request.Take}", token))
            .Map(bundles => bundles ?? Enumerable.Empty<Bundle>()));

        public string GetBundleSize(string type, decimal data)
        => (type, data) switch
            {
                (BundleType.Unlimited, _) => $"{data}mbps",
                (_, >= 1024) => $"{data / 1024}GB",
                _ => $"{data}MB"
            };

        public string GetBundleValidity(string type, int expiresInHours)
        => (type, expiresInHours) switch
        {
            (BundleType.Daily, _) => $"{expiresInHours} heure(s)",
            (BundleType.Monthly, _) => $"{expiresInHours / 720} moi(s)",
            (BundleType.Weekly, _) => $"{expiresInHours / 168} jour(s)",
            _ => $"{expiresInHours / 720} moi(s)"
        };
    }
}
