using AfriNetSharedClientLib.Auth.Services;
using AfriNetSharedClientLib.Models;
using AfriNetSharedClientLib.Models.Requests;
using System.Net.Http.Json;

namespace AfriNetSharedClientLib.Common.Bundles
{
    public interface IBundleService
    {
        TryAsync<IEnumerable<Bundle>> GetBundles(PaginationRequest request, CancellationToken token);
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
    }
}
