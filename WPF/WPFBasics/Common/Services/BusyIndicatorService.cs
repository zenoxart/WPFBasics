using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFBasics.Common.Services
{
    /// <summary>
    /// Dienst zur Verwaltung eines Busy-Indikators für asynchrone Operationen.
    /// </summary>
    public class BusyIndicatorService : INotifyPropertyChanged
    {
        private int _busyCount;

        /// <summary>
        /// Gibt an, ob aktuell eine oder mehrere Operationen laufen.
        /// </summary>
        public bool IsBusy => _busyCount > 0;

        /// <summary>
        /// Die aktuell angezeigte Busy-Nachricht.
        /// </summary>
        public string BusyMessage
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = null!;

        /// <summary>
        /// Zeigt den Busy-Indikator mit einer optionalen Nachricht an.
        /// </summary>
        /// <param name="message">Die anzuzeigende Nachricht. Standard: "Bitte warten...".</param>
        public void Show(string message = "Bitte warten...")
        {
            _busyCount++;
            BusyMessage = message;
            OnPropertyChanged(nameof(IsBusy));
        }

        /// <summary>
        /// Blendet den Busy-Indikator aus, wenn keine weiteren Operationen laufen.
        /// </summary>
        public void Hide()
        {
            if (_busyCount > 0)
                _busyCount--;
            if (_busyCount == 0)
                BusyMessage = null;
            OnPropertyChanged(nameof(IsBusy));
        }

        /// <summary>
        /// Zeigt den Busy-Indikator an und gibt ein IDisposable zurück, das beim Entsorgen den Indikator wieder ausblendet.
        /// </summary>
        /// <param name="message">Die anzuzeigende Nachricht. Standard: "Bitte warten...".</param>
        /// <returns>IDisposable, das beim Entsorgen den Busy-Indikator ausblendet.</returns>
        public IDisposable ShowScoped(string message = "Bitte warten...")
        {
            Show(message);
            return new BusyScope(this);
        }

        /// <summary>
        /// Interne Hilfsklasse zur Verwaltung des Gültigkeitsbereichs des Busy-Indikators.
        /// </summary>
        /// <remarks>
        /// Erstellt eine neue Instanz von <see cref="BusyScope"/>.
        /// </remarks>
        /// <param name="service">Die zugehörige BusyIndicatorService-Instanz.</param>
        private class BusyScope(BusyIndicatorService service) : IDisposable
        {
            private bool _disposed;

            /// <summary>
            /// Blendet den Busy-Indikator aus, wenn das Objekt entsorgt wird.
            /// </summary>
            public void Dispose()
            {
                if (!_disposed)
                {
                    service.Hide();
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Wird ausgelöst, wenn sich eine Eigenschaft geändert hat.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Benachrichtigt über eine Änderung einer Eigenschaft.
        /// </summary>
        /// <param name="name">Name der geänderten Eigenschaft.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
