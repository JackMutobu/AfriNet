using LanguageExt.Common;

namespace AfriNetLocalApi.Extensions
{
    public static class EndpointExtensions
    {
        public static Task ThrowError<TRequest, TResponse>(this Endpoint<TRequest, TResponse> endpoint , Error error) where TRequest : notnull
        {
            endpoint.ThrowError(error.Message);
            return Task.CompletedTask;
        }

        public static Task<T?> ThrowError<T,TRequest, TResponse>(this Endpoint<TRequest, TResponse> endpoint, Error error) where TRequest : notnull
        {
            endpoint.ThrowError(error.Message);
            return Task.FromResult(default(T));
        }
    }
}
