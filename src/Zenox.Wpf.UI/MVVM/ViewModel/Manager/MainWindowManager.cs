using System.Collections.ObjectModel;
using System.Windows;
using Zenox.Wpf.Core.Common.Command;
using Zenox.Wpf.Core.Common.MVVM.ViewModelSupport;
using Zenox.Wpf.Core.Common.MVVM.ViewSupport;
using Zenox.Wpf.Core.Common.Permission;
using Zenox.Wpf.Core.Common.Threading;
using Zenox.Wpf.UI.MVVM.Model;

namespace Zenox.Wpf.UI.MVVM.ViewModel.Manager
{
    public class MainWindowManager : NotificationBase
    {
        #region Eigenschaften

        /// <summary>
        /// Pool für RelayCommands, die im ViewModel verwendet werden.
        /// </summary>
        public RelayCommandPool CommandPool { get; } = [];

        /// <summary>
        /// Fortschrittsmanager für das Datagrid, verwaltet Ladefortschritt und Daten.
        /// </summary>
        public DatagridProgress<TestRecord> grdExampleData1 { get; } = new();
        /// <summary>
        /// Fortschrittsmanager für das Datagrid, verwaltet Ladefortschritt und Daten.
        /// </summary>
        public DatagridProgress<ExampleDTO> grdExampleData2 { get; } = new();

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
        public MainWindowManager()
        {
            // Fügt einen Befehl hinzu, der "Hello World" als Status setzt.
            CommandPool.Add("OpenExecutionLog", new RelayCommand(
                _ =>
                {
                    var exelogWindow = new ExecutionLogWindow();
                    //(exelogWindow.DataContext as ExecutionLogViewModel).ExecutionLogList = CommandPool.ExecutionLog.ToList();
                    exelogWindow.Show();
                },
                _ => true));

            // Fügt einen Befehl hinzu, der eine Beispiel-Inkrement-Nachricht als Status setzt.
            CommandPool.Add("ChangeLanguage", new RelayCommand(
                iso =>
                {
                    if (!string.IsNullOrEmpty(iso as string))
                    {
                        this.Kontext.Sprachen.Festlegen(iso as string);

                        if (Kontext.Sprachen.AktuelleSprache.Code == "En")
                        {

                            MessageBox.Show($"Language changed to {this.Kontext.Sprachen.AktuelleSprache.Name}!");
                        }
                        else if (Kontext.Sprachen.AktuelleSprache.Code == "De")
                        {
                            MessageBox.Show($"Sprache gewechselt zu {this.Kontext.Sprachen.AktuelleSprache.Name}!");
                        }
                    }
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
            CommandPool.Add("LoadExampleData1", new RelayCommand(
                async _ => await LoadExampleData1Async(),
                _ => !grdExampleData1.IsLoading))
            ;

            // Fügt einen Befehl hinzu, der asynchron Testdaten lädt.
            CommandPool.Add("LoadExampleData2", new RelayCommand(
                async _ => await LoadExampleData2Async(),
                _ => !grdExampleData2.IsLoading))
            ;

            // Fügt einen Befehl hinzu, der einen Admin-Befehl ausführt.
            CommandPool.Add("AdminCommand", new RelayCommand(
                _ => { Status = "Admin-Befehl ausgeführt."; },
                _ => true,
                PermissionType.Admin));
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Lädt Testdaten asynchron und aktualisiert den Fortschritt.
        /// </summary>
        /// <returns>Ein Task, der den asynchronen Ladevorgang repräsentiert.</returns>
        private async Task LoadExampleData1Async()
        {
            await grdExampleData1.LoadAsync(async progress =>
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
        /// <summary>
        /// Lädt Testdaten asynchron und aktualisiert den Fortschritt.
        /// </summary>
        /// <returns>Ein Task, der den asynchronen Ladevorgang repräsentiert.</returns>
        private async Task LoadExampleData2Async()
        {
            await grdExampleData2.LoadAsync(async progress =>
            {
                ObservableCollection<ExampleDTO> items = [];
                for (int i = 1; i <= 10; i++)
                {
                    await Task.Delay(200); // Simuliert Ladedauer
                    items.Add(new ExampleDTO { Id = i, Name = $"Example-Name {i}", Description = $"Description{new Random().Next(500)}" });
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
