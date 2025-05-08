using System.Windows;
using WPFBasics.Common.Extensions.FontMapping;

namespace UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // Alternative initialization using a direct instance creation
        var fontMapper = new FontMapper();

        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}

