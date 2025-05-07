using System.Collections.Concurrent;

namespace WPFBasics.Common.ViewModelSupport
{
    /// <summary>
    /// Stellt ViewModels zentral bereit und ermöglicht deren Wiederverwendung oder Austausch.
    /// </summary>
    public class ViewModelLocator
    {
        private static readonly ConcurrentDictionary<Type, object> _cache = new();

        /// <summary>
        /// Gibt eine Instanz des gewünschten ViewModels zurück.
        /// </summary>
        /// <typeparam name="T">Der Typ des ViewModels.</typeparam>
        /// <returns>Die Instanz des ViewModels.</returns>
        public T Get<T>() where T : class, new()
        {
            return (T)_cache.GetOrAdd(typeof(T), _ => new T());
        }

        /// <summary>
        /// Registriert eine Factory-Methode für ein ViewModel (z. B. für Dependency Injection).
        /// </summary>
        /// <typeparam name="T">Der Typ des ViewModels.</typeparam>
        /// <param name="factory">Die Factory-Methode.</param>
        public void Register<T>(Func<T> factory) where T : class
        {
            _cache[typeof(T)] = factory();
        }
    }
}
