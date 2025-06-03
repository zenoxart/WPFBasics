namespace FluentApp
{
    public class MainRootNavigationNotInitializedException : Exception
    {
        /// <summary>
        /// Erstellt eine neue Instanz der MainRootNavigationNotInitializedException.
        /// </summary>
        public MainRootNavigationNotInitializedException()
            : base("MainRootNavigation wurde nicht initialisiert.")
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz der MainRootNavigationNotInitializedException mit einer benutzerdefinierten Nachricht.
        /// </summary>
        /// <param name="message">Die benutzerdefinierte Fehlermeldung.</param>
        public MainRootNavigationNotInitializedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz der MainRootNavigationNotInitializedException mit einer benutzerdefinierten Nachricht und einer inneren Ausnahme.
        /// </summary>
        /// <param name="message">Die benutzerdefinierte Fehlermeldung.</param>
        /// <param name="innerException">Die innere Ausnahme, die die Ursache dieser Ausnahme ist.</param>
        public MainRootNavigationNotInitializedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}