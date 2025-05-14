using System.Windows;
using Zenox.Wpf.Core.Common.MVVM.FactoryInjection;
using Zenox.Wpf.UI.MVVM.ViewModel;
using Zenox.Wpf.UI.Properties;

namespace Zenox.Wpf.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected AppKontext Kontext { get; set; } = null!;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // Alternative initialization using a direct instance creation
        //var fontMapper = new FontMapper();

        //var mainWindow = new MainWindow();
        //mainWindow.Show();

        Kontext = new AppKontext();

        Kontext.Sprachen.Festlegen(Settings.Default.LocalisationISO);

        var appModel = Kontext.Produziere<AppViewModel>();

        appModel.UIAnzeigen();

    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Die aktuelle Sprache setzen
        Settings.Default.LocalisationISO = this.Kontext.Sprachen.AktuelleSprache.Code;

        // Die aktuelle Sprache speichern
        Settings.Default.Save();


        // Die Infrastruktur herunterfahren

        // Zum schluss das was sonst passiert
        base.OnExit(e);

    }
}

