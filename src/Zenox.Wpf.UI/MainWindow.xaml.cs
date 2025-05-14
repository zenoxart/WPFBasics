using System.Windows;

namespace Zenox.Wpf.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var context = DataContext;
        //DataContext = new TestViewModel();
    }
}