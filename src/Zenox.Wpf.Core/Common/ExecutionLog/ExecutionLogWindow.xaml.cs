using System.Windows;
using Zenox.Wpf.Core.Common.ExecutionLog;

namespace Zenox.Wpf.Core.Common.MVVM.ViewSupport
{
    /// <summary>
    /// Interaktionslogik für ExecutionLogWindow.xaml
    /// </summary>
    public partial class ExecutionLogWindow : Window
    {
        public ExecutionLogWindow()
        {
            InitializeComponent();

            this.DataContext = new ExecutionLogViewModel();
        }




    }
}
