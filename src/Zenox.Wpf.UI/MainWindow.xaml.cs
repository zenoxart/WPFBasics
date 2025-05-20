using System.Windows.Controls;

namespace Zenox.Wpf.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : UserControl
{
    public MainWindow()
    {
        InitializeComponent();
        var context = DataContext;
        //DataContext = new TestViewModel();
    }
}