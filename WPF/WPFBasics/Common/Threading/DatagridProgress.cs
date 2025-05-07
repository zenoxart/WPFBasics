using System.Collections.ObjectModel;
using WPFBasics.Common.ViewModelSupport;

namespace WPFBasics.Common.Threading
{
    /// <summary>
    /// Stellt einen Fortschritts-Wrapper für eine Datagrid-Datenquelle bereit, inklusive Ladefortschritt, Status und asynchronem Laden.
    /// </summary>
    /// <typeparam name="T">Der Typ der Elemente, die im Datagrid angezeigt werden.</typeparam>
    public class DatagridProgress<T> : ViewModelNotifyPropertyBase
    {

        /// <summary>
        /// Die aktuell geladenen Elemente für das Datagrid.
        /// </summary>
        public ObservableCollection<T> Items
        {
            get => field;
            private set
            {
                field = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Der aktuelle Ladefortschritt (zwischen 0.0 und 1.0).
        /// </summary>
        public double Progress
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gibt an, ob der Ladevorgang aktuell läuft.
        /// </summary>
        public bool IsLoading
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Statusnachricht zum aktuellen Ladevorgang.
        /// </summary>
        public string StatusMessage
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DatagridProgress{T}"/>-Klasse.
        /// </summary>
        public DatagridProgress()
        {
            Items = [];
            Progress = 0;
            IsLoading = false;
            StatusMessage = string.Empty;
        }

        /// <summary>
        /// Lädt die Daten asynchron und aktualisiert Fortschritt, Status und Items.
        /// </summary>
        /// <param name="loadFunc">Eine Funktion, die die Daten lädt und Fortschrittsupdates ermöglicht.</param>
        /// <param name="cancellationToken">Token zum Abbrechen des Ladevorgangs.</param>
        public async Task LoadAsync(Func<IProgress<double>, Task<ObservableCollection<T>>> loadFunc, CancellationToken cancellationToken = default)
        {
            IsLoading = true;
            Progress = 0;
            StatusMessage = "Laden...";

            Progress<double> progress = new(value =>
            {
                Progress = value;
            });

            try
            {
                ObservableCollection<T> items = await loadFunc(progress);
                if (!cancellationToken.IsCancellationRequested)
                {
                    Items = items;
                    Progress = 1.0;
                    StatusMessage = "Fertig geladen.";
                }
                else
                {
                    StatusMessage = "Laden abgebrochen.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
