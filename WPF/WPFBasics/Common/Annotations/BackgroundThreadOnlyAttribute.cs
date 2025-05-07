namespace WPFBasics.Common.Annotations
{

    /// <summary>
    /// Erzwingt, dass eine Methode nur in einem Hintergrundthread ausgeführt wird.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class BackgroundThreadOnlyAttribute : Attribute
    {
        public static void Validate()
        {
            if (Thread.CurrentThread.IsBackground == false)
            {
                throw new InvalidOperationException("Diese Methode darf nur in einem Hintergrundthread ausgeführt werden.");
            }
        }
    }
}
