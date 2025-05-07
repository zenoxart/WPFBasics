using System.Windows;

namespace WPFBasics.Common.Annotations
{

    /// <summary>
    /// Erzwingt, dass eine Methode nur im UI-Thread aufgerufen werden darf.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class UIThreadOnlyAttribute : Attribute
    {
        public static void Validate()
        {
            if (Application.Current?.Dispatcher?.CheckAccess() == false)
            {
                throw new InvalidOperationException("Diese Methode darf nur im UI-Thread aufgerufen werden.");
            }
        }
    }
}
