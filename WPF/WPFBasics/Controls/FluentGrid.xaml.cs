using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFBasics.Controls
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

        // Die Headerzeile (üblicherweise ein leeres oder ein Dummy-Item, um die Spalten zu erzeugen)
        public IEnumerable HeaderItems
        {
            get => (IEnumerable)GetValue(HeaderItemsProperty);
            set => SetValue(HeaderItemsProperty, value);
        }
        public static readonly DependencyProperty HeaderItemsProperty =
            DependencyProperty.Register(nameof(HeaderItems), typeof(IEnumerable), typeof(FluentGrid), new PropertyMetadata(null));

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
