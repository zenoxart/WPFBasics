using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFBasics.Common.ViewModelSupport
{
    /// <summary>
    /// Implementiert das INotifyPropertyChanged-Interface für eine einfache Benachrichtigung bei Eigenschaftsänderungen.
    /// </summary>
    public class ViewModelNotifyPropertyBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Tritt auf, wenn sich ein Eigenschaftswert ändert.
        /// </summary>
        /// <remarks>Dieses Ereignis wird typischerweise verwendet, um Abonnenten darüber zu informieren, dass sich ein Eigenschaftswert geändert hat. Es wird häufig in Property-Settern ausgelöst, um Datenbindung oder andere Änderungsbenachrichtigungsmechanismen zu unterstützen.</remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Löst das <see cref="PropertyChanged"/>-Ereignis aus, um Listener darüber zu informieren, dass sich ein Eigenschaftswert geändert hat.
        /// </summary>
        /// <remarks>Diese Methode wird typischerweise in Property-Settern verwendet, um Datenbindungsklienten oder andere Listener darüber zu informieren, dass ein Eigenschaftswert aktualisiert wurde. Das <see cref="CallerMemberNameAttribute"/> ermöglicht es, den Namen des aufrufenden Members automatisch als Eigenschaftsnamen zu übergeben, wodurch die Notwendigkeit entfällt, diesen explizit anzugeben.</remarks>
        /// <param name="propertyName">Der Name der Eigenschaft, die sich geändert hat. Dieser Parameter ist optional und wird standardmäßig auf den Namen des Aufrufers gesetzt, falls nicht explizit angegeben.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Aktualisiert das angegebene Feld mit einem neuen Wert und löst eine Benachrichtigung über die Eigenschaftsänderung aus, wenn sich der Wert geändert hat.
        /// </summary>
        /// <remarks>Diese Methode wird typischerweise in Property-Settern verwendet, um Feldaktualisierungen zu vereinfachen und sicherzustellen, dass Benachrichtigungen über Eigenschaftsänderungen ausgelöst werden, wenn sich der Wert ändert. Sie verwendet den Standard-EqualityComparer, um zu bestimmen, ob der neue Wert sich vom aktuellen Wert unterscheidet.</remarks>
        /// <typeparam name="T">Der Typ des zu aktualisierenden Feldes.</typeparam>
        /// <param name="field">Eine Referenz auf das zu aktualisierende Feld.</param>
        /// <param name="value">Der neue Wert, der dem Feld zugewiesen werden soll.</param>
        /// <param name="propertyName">Der Name der mit dem Feld verknüpften Eigenschaft. Dieser wird automatisch durch den Namen des aufrufenden Members bereitgestellt, falls nicht explizit angegeben.</param>
        /// <returns><see langword="true"/>, wenn der Feldwert aktualisiert und eine Benachrichtigung über die Eigenschaftsänderung ausgelöst wurde; andernfalls <see langword="false"/>, wenn der Wert unverändert blieb.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
