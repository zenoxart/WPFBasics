using System.Windows;
using System.Windows.Threading;

namespace WPFBasics.Common.Threading
{
    /// <summary>
    /// Stellt Hilfsmethoden für den Zugriff auf den UI-Dispatcher bereit.
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// Ruft den Dispatcher des UI-Threads ab.
        /// </summary>
        public static Dispatcher UIDispatcher { get; private set; }

        /// <summary>
        /// Initialisiert den <see cref="UIDispatcher"/> mit dem aktuellen Dispatcher.
        /// </summary>
        public static void Initialize()
        {
            UIDispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        /// <summary>
        /// Führt die angegebene Aktion auf dem UI-Thread aus.
        /// </summary>
        /// <param name="action">Die auszuführende Aktion.</param>
        public static void RunOnUI(Action action)
        {
            if (UIDispatcher == null)
                Initialize();

            if (UIDispatcher.CheckAccess())
                action();
            else
                UIDispatcher.Invoke(action);
        }

        /// <summary>
        /// Führt die angegebene Aktion asynchron auf dem UI-Thread aus.
        /// </summary>
        /// <param name="action">Die auszuführende Aktion.</param>
        /// <returns>Ein Task, der die Ausführung der Aktion repräsentiert.</returns>
        public static async Task RunOnUIAsync(Action action)
        {
            if (UIDispatcher == null)
                Initialize();

            if (UIDispatcher.CheckAccess())
                action();
            else
                await UIDispatcher.InvokeAsync(action);
        }
    }
}
