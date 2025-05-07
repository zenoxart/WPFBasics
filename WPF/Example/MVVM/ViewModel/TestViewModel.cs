using System.Collections.ObjectModel;
using WPFBasics.Common.Command;
using WPFBasics.Common.Threading;
using WPFBasics.Common.ViewModelSupport;

namespace Example.MVVM.ViewModel
{
    /// <summary>
    /// ViewModel für Testzwecke mit Beispielbefehlen und asynchronem Ladevorgang.
    /// </summary>
    public class TestViewModel : ViewModelNotifyPropertyBase
    {

        #region Eigenschaften

        /// <summary>
        /// Pool für RelayCommands, die im ViewModel verwendet werden.
        /// </summary>
        public RelayCommandPool CommandPool { get; } = [];

        /// <summary>
        /// Fortschrittsmanager für das Datagrid, verwaltet Ladefortschritt und Daten.
        /// </summary>
        public DatagridProgress<TestRecord> ProgressManager { get; } = new();

        /// <summary>
        /// Statusmeldung, die im UI angezeigt werden kann.
        /// </summary>
        public string Status
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
        /// Initialisiert das TestViewModel und legt die verfügbaren Befehle an.
        /// </summary>
        public TestViewModel()
        {
            // Fügt einen Befehl hinzu, der "Hello World" als Status setzt.
            CommandPool.Add("SayHello", new RelayCommand(
                _ =>
                {
                    Status = "Hello World";
                },
                _ => true));

            // Fügt einen Befehl hinzu, der eine Beispiel-Inkrement-Nachricht als Status setzt.
            CommandPool.Add("Increment", new RelayCommand(
                _ =>
                {
                    Status = "Hello 1, 2, 3, 4, 5";
                },
                _ => true));

            // Fügt einen Befehl hinzu, der den Status auf eine Reset-Nachricht setzt.
            CommandPool.Add("Reset", new RelayCommand(
                _ =>
                {
                    Status = "Reset this thing...";
                },
                _ => true));

            // Fügt einen Befehl hinzu, der asynchron Testdaten lädt.
            CommandPool.Add("LoadData", new RelayCommand(
                async _ => await LoadDataAsync(),
                _ => !ProgressManager.IsLoading));
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Lädt Testdaten asynchron und aktualisiert den Fortschritt.
        /// </summary>
        /// <returns>Ein Task, der den asynchronen Ladevorgang repräsentiert.</returns>
        private async Task LoadDataAsync()
        {
            await ProgressManager.LoadAsync(async progress =>
            {
                ObservableCollection<TestRecord> items = [];
                for (int i = 1; i <= 10; i++)
                {
                    await Task.Delay(200); // Simuliert Ladedauer
                    items.Add(new TestRecord { Name = $"Peter{i}", Address = $"New Address{new Random().Next(500)}", Age = new Random().Next(88) });
                    progress.Report(i / 10.0);
                }
                return items;
            });
        }

        #endregion
    }

    /// <summary>
    /// Repräsentiert einen Testdatensatz mit Name, Alter und Adresse.
    /// </summary>
    public record TestRecord()
    {
        /// <summary>
        /// Name der Person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alter der Person.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Adresse der Person.
        /// </summary>
        public string Address { get; set; }
    }
}
