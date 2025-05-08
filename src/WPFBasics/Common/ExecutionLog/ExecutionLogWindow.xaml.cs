using System.Windows;
using WPFBasics.Common.ExecutionLog;

namespace WPFBasics.Common.MVVM.ViewSupport
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
