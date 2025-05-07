using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WPFBasics.Common;
using WPFBasics.Common.Services;

namespace WPFBasics.Common.Connection
{
    /// <summary>
    /// Stellt Methoden für generische GraphQL-Requests bereit.
    /// </summary>
    public class GraphQLService
    {
        public LogService Logger { get; init; }
        public HttpClient HttpClient { get; init; } = new();

        /// <summary>
        /// Führt eine GraphQL-Query aus und gibt das Ergebnis als Typ T zurück.
        /// </summary>
        public async Task<T> QueryAsync<T>(string url, string query, object variables = null)
        {
            try
            {
                var request = new
                {
                    query,
                    variables
                };
                var response = await HttpClient.PostAsJsonAsync(url, request);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("data", out var data))
                {
                    return JsonSerializer.Deserialize<T>(data.GetRawText());
                }
                return default;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "GraphQLService.QueryAsync");
                Logger?.LogException(ex, "GraphQLService.QueryAsync");
                return default;
            }
        }
    }
}
