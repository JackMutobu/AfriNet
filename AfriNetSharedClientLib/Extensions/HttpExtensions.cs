using AfriNetSharedClientLib.Auth.Services;
using LanguageExt.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AfriNetSharedClientLib.Extensions
{
    public static class HttpExtensions
    {

        public async static Task<T?> Deserialize<T>(this HttpResponseMessage result) where T : class
        {
            var content = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public static EitherAsync<Error, T?> ToEitherAsync<T>(this HttpResponseMessage result, string errorMessage) where T : class
            => result.IsSuccessStatusCode ? RightAsync<Error, T?>(result.Deserialize<T>()) : LeftAsync<Error, T?>(Error.New($"{errorMessage}"));

        public static EitherAsync<Error, T> ToEitherAsync<T>(this Task<T> task, string errorMessage) where T : class
           => task.Result is not null ? RightAsync<Error, T>(task.Result) : LeftAsync<Error, T>(Error.New(errorMessage));

        public static async Task<int> ParseToInt(this HttpRequestMessage result)
            => int.Parse(await result.Content!.ReadAsStringAsync());

        public static async Task<HttpClient> SetAuthToken(this IAuthService authService, HttpClient httpClient, CancellationToken token)
        {
            var authToken = await authService.GetToken(token);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            return httpClient;
        }

        public static HttpRequestMessage DeleteRequest(this int resourceId, string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(new { id = resourceId }), Encoding.UTF8, "application/json")
            };
            return request;
        }

        public static HttpRequestMessage DeleteRequest(this string resourceId, string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(new { id = resourceId }), Encoding.UTF8, "application/json")
            };
            return request;
        }

        public static async Task<HttpResponseMessage> ToHttpResult(this HttpResponseMessage result)
        {
            if (result.IsSuccessStatusCode)
                return result;
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new Exception("Veuillez vous reconnecter");
            var errorMessage = await result.Content.ReadAsStringAsync();
            throw new Exception(errorMessage);
        }
    }
}
