using Microsoft.Win32;
using System.Windows;

namespace Zenox.Wpf.Core.Common.Services
{

    /// <summary>
    /// Bietet Methoden zum �ffnen und Speichern von Dateien �ber Dateidialoge.
    /// </summary>
    public class FileDialogService
    {
        /// <summary>
        /// �ffnet einen Dateiauswahldialog und gibt den ausgew�hlten Dateipfad zur�ck.
        /// </summary>
        /// <param name="filter">Der Filter f�r die anzuzeigenden Dateitypen (z.B. "Textdateien (*.txt)|*.txt").</param>
        /// <returns>Der vollst�ndige Pfad zur ausgew�hlten Datei oder <c>null</c>, wenn der Dialog abgebrochen wurde.</returns>
        public string OpenFile(string filter = "Alle Dateien (*.*)|*.*")
        {
            OpenFileDialog dlg = new() { Filter = filter };
            return dlg.ShowDialog(Application.Current.MainWindow) == true ? dlg.FileName : null;
        }

        /// <summary>
        /// �ffnet einen Dialog zum Speichern einer Datei und gibt den ausgew�hlten Dateipfad zur�ck.
        /// </summary>
        /// <param name="filter">Der Filter f�r die anzuzeigenden Dateitypen (z.B. "Textdateien (*.txt)|*.txt").</param>
        /// <param name="defaultFileName">Der standardm��ig vorgeschlagene Dateiname.</param>
        /// <returns>Der vollst�ndige Pfad zur zu speichernden Datei oder <c>null</c>, wenn der Dialog abgebrochen wurde.</returns>
        public string SaveFile(string filter = "Alle Dateien (*.*)|*.*", string defaultFileName = "")
        {
            SaveFileDialog dlg = new() { Filter = filter, FileName = defaultFileName };
            return dlg.ShowDialog(Application.Current.MainWindow) == true ? dlg.FileName : null;
        }
    }
}
