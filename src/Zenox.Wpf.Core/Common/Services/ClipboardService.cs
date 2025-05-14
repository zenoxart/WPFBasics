using System.Windows;
using Zenox.Wpf.Core.Common;

namespace Zenox.Wpf.Core.Common.Services
{
    /// <summary>
    /// Stellt Methoden zum Arbeiten mit der Zwischenablage bereit.
    /// </summary>
    public static class ClipboardService
    {
        /// <summary>
        /// Setzt den angegebenen Text in die Zwischenablage.
        /// </summary>
        /// <param name="text">Der zu setzende Text. Wenn null, wird ein leerer String verwendet.</param>
        public static void SetText(string text)
        {
            try
            {
                Clipboard.SetText(text ?? string.Empty);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "ClipboardService.SetText");
            }
        }

        /// <summary>
        /// Gibt den Text aus der Zwischenablage zurück.
        /// </summary>
        /// <returns>Der Text aus der Zwischenablage oder ein leerer String, falls kein Text vorhanden ist oder ein Fehler auftritt.</returns>
        public static string GetText()
        {
            try
            {
                return Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "ClipboardService.GetText");
                return string.Empty;
            }
        }

        /// <summary>
        /// Löscht den Inhalt der Zwischenablage.
        /// </summary>
        public static void Clear()
        {
            try
            {
                Clipboard.Clear();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "ClipboardService.Clear");
            }
        }
    }
}
