using System.Windows;
using Zenox.Wpf.Core.Common.MVVM.Composite;
using Zenox.Wpf.UI.MVVM.ViewModel;
using Zenox.Wpf.UI.Properties;

namespace Zenox.Wpf.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Ruft den CompositeModuleHandler ab oder legt ihn fest, der für die Verwaltung und Koordination aller Anwendungs-Module verantwortlich ist.
    /// </summary>
    protected CompositeModuleHandler ModuleHandler { get; set; } = null!;


    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // Alternative initialization using a direct instance creation
        //var fontMapper = new FontMapper();

        //var mainWindow = new MainWindow();
        //mainWindow.Show();

        ModuleHandler = new CompositeModuleHandler();

        ModuleHandler.Kontext.Sprachen.Festlegen(Settings.Default.LocalisationISO);

        var Shell = new MainAppShell();

        Shell.Show();

        ModuleHandler.LoadView<MainWindow, AppViewModel>("MainView", Shell.MainModul);

    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Die aktuelle Sprache setzen
        Settings.Default.LocalisationISO = this.ModuleHandler.Kontext.Sprachen.AktuelleSprache.Code;

        // Die aktuelle Sprache speichern
        Settings.Default.Save();


        // Die Infrastruktur herunterfahren

        // Zum schluss das was sonst passiert
        base.OnExit(e);

    }
}

