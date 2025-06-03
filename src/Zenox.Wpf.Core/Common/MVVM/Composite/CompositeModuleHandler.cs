using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Zenox.Wpf.Core.Common.MVVM.FactoryInjection;

namespace Zenox.Wpf.Core.Common.MVVM.Composite
{
    /// <summary>
    /// Verwaltet die Registrierung von Modulen sowie das dynamische Laden von Views und ViewModels in diesen Modulen.
    /// </summary>
    /// <remarks>Diese Klasse bietet Funktionalität, um benannte Module zu registrieren und Views dynamisch in diese zu laden.
    /// Views und die zugehörigen ViewModels können entweder direkt über ihre Typen oder durch Angabe ihrer Typnamen und des Assembly-Pfads geladen werden.
    /// Die geladenen Views werden als Inhalt der entsprechenden Module gesetzt und ihr DataContext wird auf das zugehörige ViewModel gesetzt.</remarks>
    /// <example> 
    /// Implementation xaml:
    ///     ContentControl x:Name="MainModul"
    ///     
    /// Implementation Code:
    ///     private readonly ModuleManager _ModuleManager = new();
    /// 
    ///     _ModuleManager.RegisterModule("MainModul", MainModul);
    ///     _ModuleManager.LoadView&lt;YourView, YourViewModel&gt;("MainModul");
    /// 
    /// </example>
    public class CompositeModuleHandler
    {
        protected readonly Dictionary<string, ContentControl> _Modules = new();

        /// <summary>
        /// Kontext für die Erzeugung von Anwendungsobjekten (z.B. ViewModels)
        /// </summary>
        public virtual AppKontext Kontext { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz des CompositeManagers mit optionalem AppKontext.
        /// </summary>
        /// <param name="appKontext">Optionaler AppKontext, ansonsten wird ein neuer erzeugt.</param>
        public CompositeModuleHandler(AppKontext? appKontext = null)
        {
            Kontext = appKontext ?? new AppKontext();
        }

        /// <summary>
        /// Registriert ein Modul mit dem angegebenen Namen und Inhalt.
        /// </summary>
        /// <remarks>Falls bereits ein Modul mit dem angegebenen Namen registriert ist, macht diese Methode nichts.</remarks>
        /// <param name="name">Der eindeutige Name des zu registrierenden Moduls. Darf nicht null oder leer sein.</param>
        /// <param name="Modul">Die <see cref="ContentControl"/>-Instanz, die den Inhalt des Moduls repräsentiert. Darf nicht null sein.</param>
        public void RegisterModule(string name, ContentControl Modul)
        {
            if (!_Modules.ContainsKey(name))
                _Modules.Add(name, Modul);
        }

        /// <summary>
        /// Lädt eine View und das zugehörige ViewModel in die angegebene Module.
        /// Das ViewModel wird über den AppKontext erzeugt, sofern es von AppObject ableitet.
        /// </summary>
        /// <typeparam name="TView">Der Typ der View (muss FrameworkElement sein).</typeparam>
        /// <typeparam name="TViewModel">Der Typ des ViewModels.</typeparam>
        /// <param name="moduleName">Der Name der ZielModule.</param>
        public virtual void LoadView<TView, TViewModel>(
            string moduleName,
            ContentControl Modul = null)
            where TView : FrameworkElement, new()
            where TViewModel : AppObject, new()
        {
            try
            {
                TViewModel viewModel;

                // Wenn das ViewModel von AppObject ableitet, über AppKontext erzeugen
                if (typeof(IAppObject).IsAssignableFrom(typeof(TViewModel)))
                {
                    viewModel = (TViewModel)(object)Kontext.GetManager<TViewModel>();
                }
                else
                {
                    viewModel = new TViewModel();
                }

                if (!_Modules.TryGetValue(moduleName, out var Module))
                {
                    RegisterModule(moduleName, Modul);

                    _Modules.TryGetValue(moduleName, out Module);
                }

                var view = new TView();


                view.DataContext = viewModel;
                Module.Content = view;
            }
            catch (Exception ex)
            {
                Kontext.Log.OnFehlerAufgetreten(new FehlerAufgetretenEventArgs(ex));
            }
        }
        /// <summary>
        /// Lädt eine View in die angegebene Module (ViewModel muss im Nachhinein hinzugebunden werden!).
        /// Das ViewModel wird über den AppKontext erzeugt, sofern es von AppObject ableitet.
        /// </summary>
        /// <typeparam name="TView">Der Typ der View (muss FrameworkElement sein).</typeparam>
        /// <typeparam name="TViewModel">Der Typ des ViewModels.</typeparam>
        /// <param name="moduleName">Der Name der ZielModule.</param>
        public void LoadView<TView>(
            string moduleName,
            ContentControl Modul = null)
            where TView : FrameworkElement, new()
        {
            try
            {

                if (!_Modules.TryGetValue(moduleName, out var Module))
                {
                    RegisterModule(moduleName, Modul);

                    _Modules.TryGetValue(moduleName, out Module);
                }

                var view = new TView();

                Module.Content = view;
            }
            catch (Exception ex)
            {
                Kontext.Log.OnFehlerAufgetreten(new FehlerAufgetretenEventArgs(ex));
            }
        }


        /// <summary>
        /// Lädt eine View und das zugehörige ViewModel aus einer angegebenen Assembly in das angegebene Modul.
        /// Das ViewModel wird über den AppKontext erzeugt, sofern es von AppObject ableitet.
        /// </summary>
        /// <param name="moduleName">Der Name des Zielmoduls.</param>
        /// <param name="viewTypeName">Der vollständige Typname der View (inkl. Namespace).</param>
        /// <param name="viewModelTypeName">Der vollständige Typname des ViewModels (inkl. Namespace).</param>
        /// <param name="assemblyPath">Der Pfad zur Assembly, die View und ViewModel enthält.</param>
        public void LoadView(
            string moduleName,
            string viewTypeName,
            string viewModelTypeName,
            string assemblyPath)
        {
            try
            {

                var assembly = Assembly.LoadFrom(assemblyPath);
                var viewType = assembly.GetType(viewTypeName);
                var viewModelType = assembly.GetType(viewModelTypeName);

                ;

                if (!_Modules.TryGetValue(moduleName, out var modul))
                    RegisterModule(moduleName, Activator.CreateInstance(viewType) as UserControl);


                if (viewType == null)
                    throw new ArgumentException($"View-Typ '{viewTypeName}' nicht in Assembly gefunden.");
                if (viewModelType == null)
                    throw new ArgumentException($"ViewModel-Typ '{viewModelTypeName}' nicht in Assembly gefunden.");

                var view = (FrameworkElement)Activator.CreateInstance(viewType)!;
                object? viewModel;

                // Prüfen, ob das ViewModel von AppObject ableitet
                if (typeof(AppObject).IsAssignableFrom(viewModelType))
                {
                    // AppKontext.GetManager<T> benötigt einen generischen Typ zur Compilezeit.
                    // Daher wird hier per Reflection aufgerufen:
                    var getManagerMethod = typeof(AppKontext).GetMethod("GetManager")!.MakeGenericMethod(viewModelType);
                    viewModel = getManagerMethod.Invoke(Kontext, null);
                }
                else
                {
                    viewModel = Activator.CreateInstance(viewModelType);
                }

                view.DataContext = viewModel;
                modul.Content = view;
            }
            catch (Exception ex)
            {
                Kontext.Log.OnFehlerAufgetreten(new FehlerAufgetretenEventArgs(ex));
            }
        }

    }
}
