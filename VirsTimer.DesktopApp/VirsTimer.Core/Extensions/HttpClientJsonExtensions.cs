using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VirsTimer.Core.Extensions
{
    /// <summary>
    /// <see cref="HttpClient"/> json operation extensions.
    /// </summary>
    public static class HttpClientJsonExtensions
    {
        /// <summary>
        /// Performs PATCH request with <paramref name="value"/> as JSON.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(
            this HttpClient client,
            string? requestUri,
            TValue value,
            JsonSerializerOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            using var content = JsonContent.Create(value, options: options);
            return client.PatchAsync(requestUri, content, cancellationToken);
        }
    }
}