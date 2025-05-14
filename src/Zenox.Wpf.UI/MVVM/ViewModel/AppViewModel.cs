using Zenox.Wpf.Core.Common.MVVM.FactoryInjection;
using Zenox.Wpf.Core.Common.MVVM.ViewModelSupport;
using Zenox.Wpf.UI.MVVM.ViewModel.Manager;

namespace Zenox.Wpf.UI.MVVM.ViewModel
{
    /// <summary>
    /// Definiert das Haupt-ViewModel der Anwendung
    /// </summary>
    /// <remarks>
    /// Integriert ein Factory-Pattern für Dependency-Injection
    /// </remarks>
    public class AppViewModel : NotificationBase
    {
        /// <summary>
        /// Started die Haupt-Ansicht
        /// </summary>
        /// <remarks>
        /// Hängt den Kontext and den DataContext
        /// </remarks>
        public void UIAnzeigen()
        {
            this.StartMelden();
            try
            {
                // Ein View-Objekt des gewünschten Typs

                if (System.Activator
                    .CreateInstance(
                        System.Type.GetType(
                            typeName: Properties.Settings.Default.HauptUITyp)!)
                    is not
                        System.Windows.Window NeuesFenster)
                {
                    throw new Exception("Beim Konfigurationswert HauptUITYp " +
                        "muss es sich um ein Window handeln!");
                }

                // Dieses mit dem ViewModel verknüpfen
                NeuesFenster.DataContext = this;

                // TODO:Initialisieren und Binden (Ersatz für Load Ereignis)

                // Die neue View anzeigen
                NeuesFenster.Show();

            }
            catch (Exception ex)
            {
                this.OnFehlerAufgetreten(
                    new FehlerAufgetretenEventArgs(ex));
            }

            this.EndeMelden();
        }



        /// <summary>
        /// Ruft den MainWindow-Manager ab
        /// </summary>
        public MainWindowManager MainWindow
        {
            get
            {
                if (field == null)
                {
                    field = this.Kontext.Produziere<MainWindowManager>();
                }

                return field;
            }
        }

    }
}
