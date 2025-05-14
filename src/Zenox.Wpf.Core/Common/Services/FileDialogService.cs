using Microsoft.Win32;
using System.Windows;

namespace Zenox.Wpf.Core.Common.Services
{

    /// <summary>
    /// Bietet Methoden zum Öffnen und Speichern von Dateien über Dateidialoge.
    /// </summary>
    public class FileDialogService
    {
        /// <summary>
        /// Öffnet einen Dateiauswahldialog und gibt den ausgewählten Dateipfad zurück.
        /// </summary>
        /// <param name="filter">Der Filter für die anzuzeigenden Dateitypen (z.B. "Textdateien (*.txt)|*.txt").</param>
        /// <returns>Der vollständige Pfad zur ausgewählten Datei oder <c>null</c>, wenn der Dialog abgebrochen wurde.</returns>
        public string OpenFile(string filter = "Alle Dateien (*.*)|*.*")
        {
            OpenFileDialog dlg = new() { Filter = filter };
            return dlg.ShowDialog(Application.Current.MainWindow) == true ? dlg.FileName : null;
        }

        /// <summary>
        /// Öffnet einen Dialog zum Speichern einer Datei und gibt den ausgewählten Dateipfad zurück.
        /// </summary>
        /// <param name="filter">Der Filter für die anzuzeigenden Dateitypen (z.B. "Textdateien (*.txt)|*.txt").</param>
        /// <param name="defaultFileName">Der standardmäßig vorgeschlagene Dateiname.</param>
        /// <returns>Der vollständige Pfad zur zu speichernden Datei oder <c>null</c>, wenn der Dialog abgebrochen wurde.</returns>
        public string SaveFile(string filter = "Alle Dateien (*.*)|*.*", string defaultFileName = "")
        {
            SaveFileDialog dlg = new() { Filter = filter, FileName = defaultFileName };
            return dlg.ShowDialog(Application.Current.MainWindow) == true ? dlg.FileName : null;
        }
    }
}
