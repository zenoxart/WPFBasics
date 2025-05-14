namespace Zenox.Wpf.Core.Common.Annotations
{

    /// <summary>
    /// Markiert eine Klasse oder ein Objekt, das automatisch entsorgt werden soll.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class AutoDisposeAttribute : Attribute
    {
        /// <summary>
        /// Führt die automatische Entsorgung des Objekts durch, falls es IDisposable implementiert.
        /// </summary>
        /// <param name="obj">Das zu entsorgende Objekt.</param>
        public static void DisposeIfRequired(object obj)
        {
            if (obj is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
