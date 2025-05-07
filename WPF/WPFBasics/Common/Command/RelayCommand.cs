using System.Windows.Input;

namespace WPFBasics.Common.Command
{
    /// <summary>
    /// Stellt einen Befehl dar, der ausgeführt werden kann und dessen Ausführbarkeit anhand einer angegebenen Bedingung bestimmt wird.
    /// </summary>
    /// <remarks>Diese Klasse wird typischerweise verwendet, um eine Aktion und deren Ausführungslogik zu kapseln, wodurch die Verwendung von Befehlen im MVVM (Model-View-ViewModel)-Muster ermöglicht wird. Die Ausführbarkeit des Befehls wird durch ein Prädikat bestimmt, falls angegeben, oder ist standardmäßig immer möglich.</remarks>
    public partial class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Initialisiert einen neuen Befehl mit der angegebenen Ausführungs- und Prüf-Logik.
        /// </summary>
        /// <param name="execute">Die auszuführende Aktion.</param>
        /// <param name="canExecute">Optional: Prädikat, das bestimmt, ob der Befehl ausgeführt werden kann.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Prüft, ob der Befehl ausgeführt werden kann.
        /// </summary>
        /// <param name="parameter">Optionaler Parameter.</param>
        /// <returns>True, wenn der Befehl ausgeführt werden kann, sonst false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Führt die hinterlegte Aktion aus.
        /// </summary>
        /// <param name="parameter">Optionaler Parameter.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Wird ausgelöst, wenn sich die Ausführbarkeit des Befehls ändert.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value; remove => CommandManager.RequerySuggested -= value;
        }
    }
}
