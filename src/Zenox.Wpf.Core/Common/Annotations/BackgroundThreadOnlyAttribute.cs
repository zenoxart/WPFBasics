namespace Zenox.Wpf.Core.Common.Annotations
{

    /// <summary>
    /// Erzwingt, dass eine Methode nur in einem Hintergrundthread ausgeführt wird.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class BackgroundThreadOnlyAttribute : Attribute
    {
        /// <summary>
        /// Überprüft, ob die aktuelle Methode in einem Hintergrundthread ausgeführt wird.
        /// Wirft eine InvalidOperationException, wenn dies nicht der Fall ist.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Wird ausgelöst, wenn die Methode nicht in einem Hintergrundthread ausgeführt wird.
        /// </exception>
        public static void Validate()
        {
            if (Thread.CurrentThread.IsBackground == false)
            {
                throw new InvalidOperationException("Diese Methode darf nur in einem Hintergrundthread ausgeführt werden.");
            }
        }
    }
}
