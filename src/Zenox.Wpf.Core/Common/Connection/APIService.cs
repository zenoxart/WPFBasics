using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Zenox.Wpf.Core.Common;
using Zenox.Wpf.Core.Common.Services;

namespace Zenox.Wpf.Core.Common.Connection
{
    /// <summary>
    /// Stellt Methoden für generische HTTP-API-Aufrufe bereit.
    /// </summary>
    public class APIService
    {
        /// <summary>
        /// Logger für Fehler und Informationen.
        /// </summary>
        public LogService Logger { get; init; }

        /// <summary>
        /// HttpClient für API-Aufrufe.
        /// </summary>
        public HttpClient HttpClient { get; init; } = new();

        /// <summary>
        /// Führt einen GET-Request aus und gibt das Ergebnis als Typ T zurück.
        /// </summary>
        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                return await HttpClient.GetFromJsonAsync<T>(url);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "APIService.GetAsync");
                Logger?.LogException(ex, "APIService.GetAsync");
                return default;
            }
        }

        /// <summary>
        /// Führt einen POST-Request aus und gibt das Ergebnis als Typ TResult zurück.
        /// </summary>
        public async Task<TResult> PostAsync<TRequest, TResult>(string url, TRequest data)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync(url, data);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<TResult>();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "APIService.PostAsync");
                Logger?.LogException(ex, "APIService.PostAsync");
                return default;
            }
        }
    }
}
