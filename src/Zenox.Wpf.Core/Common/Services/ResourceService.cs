using System.Windows;
using Zenox.Wpf.Core.Common;

namespace Zenox.Wpf.Core.Common.Services
{
    /// <summary>
    /// Stellt Methoden zum Abrufen von Ressourcen aus der aktuellen WPF-Anwendung bereit.
    /// </summary>
    public static class ResourceService
    {
        /// <summary>
        /// Sucht eine Ressource anhand des angegebenen Schlüssels.
        /// </summary>
        /// <param name="key">Der Schlüssel der Ressource.</param>
        /// <returns>Die gefundene Ressource oder <c>null</c>, wenn keine Ressource gefunden wurde.</returns>
        public static object GetResource(string key)
        {
            try
            {
                return Application.Current.TryFindResource(key);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "ResourceService.GetResource");
                return null;
            }
        }

        /// <summary>
        /// Sucht eine Ressource anhand des angegebenen Schlüssels und gibt sie als angegebenen Typ zurück.
        /// </summary>
        /// <typeparam name="T">Der erwartete Typ der Ressource.</typeparam>
        /// <param name="key">Der Schlüssel der Ressource.</param>
        /// <returns>Die gefundene Ressource als Typ <typeparamref name="T"/> oder den Standardwert des Typs, wenn keine Ressource gefunden wurde.</returns>
        public static T GetResource<T>(string key)
        {
            try
            {
                object resource = Application.Current.TryFindResource(key);
                return resource is T t ? t : default;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "ResourceService.GetResource<T>");
                return default;
            }
        }
    }
}
