using System.Collections;
using System.Diagnostics;
using System.Windows.Input;

namespace WPFBasics.Common.Command
{
    /// <summary>
    /// Verwaltet eine Sammlung von RelayCommands, die über eindeutige Schlüssel referenziert werden können.
    /// Ermöglicht das einfache Binden von Commands anhand ihres Schlüssels, z.B. in MVVM-Szenarien.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public class RelayCommandPool : IEnumerable<KeyValuePair<string, RelayCommand>>
    {
        private readonly Dictionary<string, RelayCommand> _commands = [];

        /// <summary>
        /// Fügt einen neuen RelayCommand mit dem angegebenen Schlüssel hinzu.
        /// </summary>
        /// <param name="key">Der eindeutige Schlüssel für den Command.</param>
        /// <param name="execute">Die auszuführende Aktion.</param>
        /// <param name="canExecute">Optional: Predicate, das bestimmt, ob der Command ausgeführt werden kann.</param>
        /// <exception cref="ArgumentException">Wenn der Schlüssel bereits existiert.</exception>
        public void Add(string key, Action<object> execute, Predicate<object> canExecute = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (_commands.ContainsKey(key))
                throw new ArgumentException($"Ein Command mit dem Schlüssel '{key}' existiert bereits.", nameof(key));

            _commands[key] = new RelayCommand(execute, canExecute);
        }

        /// <summary>
        /// Fügt einen neuen RelayCommand mit dem angegebenen Schlüssel hinzu.
        /// </summary>
        /// <param name="key">Der eindeutige Schlüssel für den Command.</param>
        /// <param name="command">Der RelayCommand.</param>
        /// <exception cref="ArgumentException">Wenn der Schlüssel bereits existiert.</exception>
        public void Add(string key, RelayCommand command)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (_commands.ContainsKey(key))
                throw new ArgumentException($"Ein Command mit dem Schlüssel '{key}' existiert bereits.", nameof(key));

            _commands[key] = command;
        }

        /// <summary>
        /// Gibt den RelayCommand für den angegebenen Schlüssel zurück.
        /// </summary>
        /// <param name="key">Der eindeutige Schlüssel.</param>
        /// <returns>Der zugehörige RelayCommand.</returns>
        /// <exception cref="KeyNotFoundException">Wenn kein Command mit dem Schlüssel existiert.</exception>
        public ICommand this[string key] => !_commands.TryGetValue(key, out RelayCommand command)
                    ? throw new KeyNotFoundException($"Kein Command mit dem Schlüssel '{key}' gefunden.")
                    : (ICommand)command;

        /// <summary>
        /// Prüft, ob ein Command mit dem angegebenen Schlüssel existiert.
        /// </summary>
        public bool Contains(string key)
        {
            return _commands.ContainsKey(key);
        }

        /// <summary>
        /// Entfernt einen Command anhand seines Schlüssels.
        /// </summary>
        /// <param name="key">Der eindeutige Schlüssel.</param>
        /// <returns>True, wenn der Command entfernt wurde, sonst false.</returns>
        public bool Remove(string key)
        {
            return _commands.Remove(key);
        }

        /// <summary>
        /// Entfernt alle gespeicherten Commands.
        /// </summary>
        public void Clear()
        {
            _commands.Clear();
        }

        /// <summary>
        /// Gibt die Anzahl der gespeicherten Commands zurück.
        /// </summary>
        public int Count => _commands.Count;

        /// <summary>
        /// Gibt eine schreibgeschützte Auflistung aller Schlüssel zurück.
        /// </summary>
        public IReadOnlyCollection<string> Keys => _commands.Keys;

        /// <summary>
        /// Gibt eine schreibgeschützte Auflistung aller RelayCommands zurück.
        /// </summary>
        public IReadOnlyCollection<RelayCommand> Values => _commands.Values;

        /// <summary>
        /// Versucht, einen Command anhand des Schlüssels zu holen.
        /// </summary>
        /// <param name="key">Der eindeutige Schlüssel.</param>
        /// <param name="command">Der gefundene Command oder null.</param>
        /// <returns>True, wenn gefunden, sonst false.</returns>
        public bool TryGetCommand(string key, out RelayCommand command)
        {
            return _commands.TryGetValue(key, out command);
        }

        /// <summary>
        /// Gibt eine lesbare Zeichenkette mit allen Schlüsseln zurück.
        /// </summary>
        public override string ToString()
        {
            return $"RelayCommandPool (Count={Count}): [{string.Join(", ", _commands.Keys)}]";
        }

        /// <summary>
        /// Gibt einen Enumerator für die gespeicherten Commands zurück.
        /// </summary>
        public IEnumerator<KeyValuePair<string, RelayCommand>> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Erweiterungsmethoden für RelayCommandPool.
    /// </summary>
    public static class RelayCommandPoolExtensions
    {
        /// <summary>
        /// Führt den Command mit dem angegebenen Schlüssel aus, falls vorhanden.
        /// </summary>
        /// <param name="pool">Der RelayCommandPool.</param>
        /// <param name="key">Der Schlüssel.</param>
        /// <param name="parameter">Optionales Parameterobjekt.</param>
        /// <returns>True, wenn ausgeführt, sonst false.</returns>
        public static bool ExecuteIfExists(this RelayCommandPool pool, string key, object parameter = null)
        {
            RelayCommand command;
            if (pool.TryGetCommand(key, out command) && command.CanExecute(parameter))
            {
                command.Execute(parameter);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gibt alle Schlüssel als Liste zurück.
        /// </summary>
        public static List<string> GetAllKeys(this RelayCommandPool pool)
        {
            return pool.Keys.ToList();
        }
    }
}
