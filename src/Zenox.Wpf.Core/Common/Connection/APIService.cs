using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Zenox.Wpf.Core.Common;
using Zenox.Wpf.Core.Common.Services;

namespace Zenox.Wpf.Core.Common.Connection
{
    /// <summary>
    /// Stellt Methoden f�r generische HTTP-API-Aufrufe bereit.
    /// </summary>
    public class APIService
    {
        /// <summary>
        /// Logger f�r Fehler und Informationen.
        /// </summary>
        public LogService Logger { get; init; }

        /// <summary>
        /// HttpClient f�r API-Aufrufe.
        /// </summary>
        public HttpClient HttpClient { get; init; } = new();

        /// <summary>
        /// F�hrt einen GET-Request aus und gibt das Ergebnis als Typ T zur�ck.
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
        /// F�hrt einen POST-Request aus und gibt das Ergebnis als Typ TResult zur�ck.
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
