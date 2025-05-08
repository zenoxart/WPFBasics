using System.Windows.Controls;
using WPFBasics.Common.ViewModelSupport;

namespace WPFBasics.Common.Handler
{
    /// <summary>
    /// Stellt einen Menü-Handler für das Verwalten & Laden 
    /// von dynamischen generierten Menü-Punkten
    /// </summary>
    public class MenuHandler : ViewModelNotifyPropertyBase
    {

        /// <summary>
        /// Definiert das zu behandelnde Menü
        /// TODO: einen RelayCommandPool benutzen
        /// </summary>
        public Menu Items
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Läd die Menü-Items aus dem angegebenen Pfad eines Ressource-Dictionary
        /// </summary>
        public void LoadItemsFromResDict(string path)
        {
            /// Lade aus dem Resource-File

            /// Überprüfe ob das Element noch nicht geladen wurde
            /// Hinterlege die Commands sowie die Views in Menü-Items
        }
    }
}