using System.IO;

namespace WPFBasics.Common.Services
{
    /// <summary>
    /// Definiert die verschiedenen Log-Level für die Protokollierung.
    /// </summary>
    public enum LogLevel { Debug, Info, Warning, Error, Fatal }

    /// <summary>
    /// Bietet eine einfache Logging-Funktionalität für die Anwendung.
    /// </summary>
    public class LogService
    {
        private readonly string _logFilePath;
        private readonly object _lock = new();

        /// <summary>
        /// Gibt das minimale Log-Level an, das protokolliert werden soll.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="LogService"/>-Klasse.
        /// </summary>
        /// <param name="logFilePath">Der Pfad zur Logdatei. Standard ist "application.log".</param>
        public LogService(string logFilePath = "application.log")
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFilePath);
        }

        /// <summary>
        /// Schreibt eine Log-Nachricht in die Logdatei, sofern das Level ausreichend ist.
        /// </summary>
        /// <param name="message">Die zu protokollierende Nachricht.</param>
        /// <param name="level">Das Log-Level der Nachricht. Standard ist Info.</param>
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            if (level < MinimumLevel) return;
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch
            {
                // Logging darf keine Exception werfen, sonst Endlosschleife möglich
            }
        }

        /// <summary>
        /// Protokolliert eine Exception mit optionalem Kontext.
        /// </summary>
        /// <param name="ex">Die aufgetretene Exception.</param>
        /// <param name="context">Optionaler Kontext zur Exception.</param>
        public void LogException(Exception ex, string context = null)
        {
            Log($"{context ?? "Exception"}: {ex}", LogLevel.Error);
        }
    }
}
