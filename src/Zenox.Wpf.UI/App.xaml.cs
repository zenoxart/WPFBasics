using System.Windows;
using Zenox.Wpf.Core.Common.MVVM.FactoryInjection;
using Zenox.Wpf.UI.MVVM.ViewModel;

namespace Zenox.Wpf.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // Alternative initialization using a direct instance creation
        //var fontMapper = new FontMapper();

        //var mainWindow = new MainWindow();
        //mainWindow.Show();

        var appKontext = new AppKontext();

        var appModel = appKontext.Produziere<AppViewModel>();

        appModel.UIAnzeigen();

    }
}

