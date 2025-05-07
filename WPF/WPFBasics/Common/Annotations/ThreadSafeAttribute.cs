namespace WPFBasics.Common.Annotations
{

    /// <summary>
    /// Markiert eine Methode oder Klasse als threadsicher.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class ThreadSafeAttribute : Attribute
    {
    }
}
