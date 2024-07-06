using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadAsStringAndDeserializeAsync<T>(this HttpContent content, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            return (await content.ReadAsStringAsync(cancellationToken)).ToObject<T>();
        }

        public static async Task<TSuccess> ReturnOrThrowAsync<TSuccess, TError>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new() where TSuccess : class, new()
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnAuthorizedException();
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAndDeserializeAsync<TError>(cancellationToken);
                throw new ServiceException(errorResponse.Message, response.StatusCode, errorResponse.Errors);
            }

            return await response.Content.ReadAsStringAndDeserializeAsync<TSuccess>(cancellationToken);
        }

        public static async Task ThrowIfErroredAsync<TError>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new()
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnAuthorizedException();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAndDeserializeAsync<TError>(cancellationToken);
                throw new ServiceException(errorResponse.Message, response.StatusCode, errorResponse.Errors);
            }
        }

        public static async Task<TSuccess> GetAndReturnOrThrowAsync<TSuccess, TError>(this HttpClient client, string url, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new() where TSuccess : class, new()
        {
            var response = await client.GetAsync(url, cancellationToken);
            return await response.ReturnOrThrowAsync<TSuccess, TError>(cancellationToken);
        }

        public static async Task<TSuccess> PostAndReturnOrThrowAsync<TSuccess, TError>(this HttpClient client, string url, object data, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new() where TSuccess : class, new()
        {
            var content = new StringContent(data.ToJson(), Encoding.UTF8, MediaTypes.Json);

            var response = await client.PostAsync(url, content, cancellationToken);

            return await response.ReturnOrThrowAsync<TSuccess, TError>(cancellationToken);
        }

        public static async Task<TSuccess> PutAndReturnOrThrowAsync<TSuccess, TError>(this HttpClient client, string url, object data, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new() where TSuccess : class, new()
        {
            var content = new StringContent(data.ToJson(), Encoding.UTF8, MediaTypes.Json);

            var response = await client.PutAsync(url, content, cancellationToken);

            return await response.ReturnOrThrowAsync<TSuccess, TError>(cancellationToken);
        }

        public static async Task PutOrThrowAsync<TError>(this HttpClient client, string url, object data, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new()
        {
            var content = new StringContent(data.ToJson(), Encoding.UTF8, MediaTypes.Json);

            var response = await client.PutAsync(url, content, cancellationToken);

            await response.ThrowIfErroredAsync<TError>(cancellationToken);
        }

        public static async Task SendOrThrowAsync<TError>(this HttpClient client, string url, object data, HttpMethod method, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new()
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(data.ToJson(), Encoding.UTF8, MediaTypes.Json)
            };

            var response = await client.SendAsync(request, cancellationToken);

            await response.ThrowIfErroredAsync<TError>(cancellationToken);
        }

        public static async Task DeleteOrThrowAsync<TError>(this HttpClient client, string url, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new()
        {
            var response = await client.DeleteAsync(url, cancellationToken);

            await response.ThrowIfErroredAsync<TError>(cancellationToken);
        }

        public static async Task<TSuccess> SendAndReturnOrThrowAsync<TSuccess, TError>(this HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken = default)
            where TError : ApiErrorResponse, new() where TSuccess : class, new()
        {
            var response = await client.SendAsync(request, cancellationToken);

            return await response.ReturnOrThrowAsync<TSuccess, TError>(cancellationToken);
        }
    }
}
