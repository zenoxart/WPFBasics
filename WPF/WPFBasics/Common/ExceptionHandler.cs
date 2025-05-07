using System.Windows;
using WPFBasics.Common.Services;

namespace WPFBasics.Common
{

    ///<summary>
    /// Stellt Methoden zum globalen Behandeln von Ausnahmen bereit.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Gibt den Logger an, der f�r das Protokollieren von Ausnahmen verwendet wird.
        /// </summary>
        public static LogService Logger { get; set; }

        /// <summary>
        /// Registriert globale Handler f�r nicht behandelte Ausnahmen in der Anwendung.
        /// </summary>
        public static void RegisterGlobalHandlers()
        {
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                Logger?.LogException(e.Exception, "DispatcherUnhandledException");
                MessageBox.Show("Ein unerwarteter Fehler ist aufgetreten:\n" + e.Exception.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                    Logger?.LogException(ex, "AppDomain.UnhandledException");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Logger?.LogException(e.Exception, "UnobservedTaskException");
                e.SetObserved();
            };
        }
    }
}
