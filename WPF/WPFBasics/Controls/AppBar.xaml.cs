using System;
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
    /// Stellt eine benutzerdefinierte AppBar für WPF-Anwendungen bereit, 
    /// die grundlegende Fenstersteuerungsfunktionen wie Minimieren, Maximieren und Schließen ermöglicht.
    /// Zusätzlich kann das Fenster durch Ziehen der AppBar verschoben werden.
    /// </summary>
    public partial class AppBar : UserControl
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="AppBar"/>-Klasse.
        /// Registriert Ereignisse für das Verschieben und Maximieren des Fensters durch die AppBar.
        /// </summary>
        public AppBar()
        {
            InitializeComponent();
            // MouseLeftButtonDown direkt registrieren, damit DragMove immer funktioniert
            MouseLeftButtonDown += AppBar_MouseLeftButtonDown;
        }

        private void AppBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Prüfe, ob das Original-Source ein Button ist, dann kein DragMove
            if (e.OriginalSource is DependencyObject depObj)
            {
                var parent = depObj;
                while (parent != null)
                {
                    if (parent is Button)
                        return;
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            var window = Window.GetWindow(this);
            if (window != null)
            {
                if (e.ClickCount == 2)
                {
                    ToggleMaximize(window);
                }
                else
                {
                    window.DragMove();
                }
            }
        }

        /// <summary>
        /// Minimiert das übergeordnete Fenster, wenn der Minimieren-Button angeklickt wird.
        /// </summary>
        /// <param name="sender">Das auslösende Steuerelement.</param>
        /// <param name="e">Ereignisdaten.</param>
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Maximiert oder stellt das übergeordnete Fenster wieder her, wenn der Maximieren-Button angeklickt wird.
        /// </summary>
        /// <param name="sender">Das auslösende Steuerelement.</param>
        /// <param name="e">Ereignisdaten.</param>
        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            ToggleMaximize(window);
        }

        /// <summary>
        /// Schließt das übergeordnete Fenster, wenn der Schließen-Button angeklickt wird.
        /// </summary>
        /// <param name="sender">Das auslösende Steuerelement.</param>
        /// <param name="e">Ereignisdaten.</param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        /// <summary>
        /// Wechselt den Fensterstatus zwischen maximiert und normal.
        /// </summary>
        /// <param name="window">Das zu maximierende oder wiederherzustellende Fenster.</param>
        private void ToggleMaximize(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;
            else
                window.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// Ändert die Hintergrundfarbe des Schließen-Buttons beim Überfahren mit der Maus.
        /// </summary>
        /// <param name="sender">Der Schließen-Button.</param>
        /// <param name="e">Ereignisdaten der Maus.</param>
        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromRgb(232, 17, 35));
        }

        /// <summary>
        /// Setzt die Hintergrundfarbe des Schließen-Buttons zurück, wenn die Maus den Button verlässt.
        /// </summary>
        /// <param name="sender">Der Schließen-Button.</param>
        /// <param name="e">Ereignisdaten der Maus.</param>
        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }
    }
}
