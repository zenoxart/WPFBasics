using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFBasics.Common.Services
{
    /// <summary>
    /// Dienst zur Verwaltung eines Busy-Indikators f�r asynchrone Operationen.
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
        /// Zeigt den Busy-Indikator an und gibt ein IDisposable zur�ck, das beim Entsorgen den Indikator wieder ausblendet.
        /// </summary>
        /// <param name="message">Die anzuzeigende Nachricht. Standard: "Bitte warten...".</param>
        /// <returns>IDisposable, das beim Entsorgen den Busy-Indikator ausblendet.</returns>
        public IDisposable ShowScoped(string message = "Bitte warten...")
        {
            Show(message);
            return new BusyScope(this);
        }

        /// <summary>
        /// Interne Hilfsklasse zur Verwaltung des G�ltigkeitsbereichs des Busy-Indikators.
        /// </summary>
        /// <remarks>
        /// Erstellt eine neue Instanz von <see cref="BusyScope"/>.
        /// </remarks>
        /// <param name="service">Die zugeh�rige BusyIndicatorService-Instanz.</param>
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
        /// Wird ausgel�st, wenn sich eine Eigenschaft ge�ndert hat.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Benachrichtigt �ber eine �nderung einer Eigenschaft.
        /// </summary>
        /// <param name="name">Name der ge�nderten Eigenschaft.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
