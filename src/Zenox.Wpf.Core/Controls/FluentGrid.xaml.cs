using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zenox.Wpf.Core.Controls
{
    /// <summary>
    /// Interaktionslogik für FluentGrid.xaml
    /// </summary>
    public partial class FluentGrid : UserControl
    {
        public FluentGrid()
        {
            InitializeComponent();
        }

        // Die eigentlichen Datenzeilen
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(FluentGrid), new PropertyMetadata(null));

        // Fortschritt (0..1)
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register(nameof(Progress), typeof(double), typeof(FluentGrid), new PropertyMetadata(0.0));

        // Lädt gerade?
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(FluentGrid), new PropertyMetadata(false));

        // LoadDataCommand DependencyProperty
        public ICommand LoadDataCommand
        {
            get => (ICommand)GetValue(LoadDataCommandProperty);
            set => SetValue(LoadDataCommandProperty, value);
        }
        public static readonly DependencyProperty LoadDataCommandProperty =
            DependencyProperty.Register(nameof(LoadDataCommand), typeof(ICommand), typeof(FluentGrid), new PropertyMetadata(null));
    }
}
