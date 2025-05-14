namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection
{
    #region Typ-Deklaration Ereignis-Behandler Methode

    /// <summary>
    /// Stellt die Methode bereit, die
    /// das FehlerAufgetreten Ereignis behandelt
    /// </summary>
    /// <param name="sender">Verweis auf das Objekt,
    /// das diese Methode aufruft</param>
    /// <param name="e">Ereignisdaten</param>
    public delegate void FehlerAufgetretenEventHandler(
                            object sender,
                            FehlerAufgetretenEventArgs e);


    #endregion Typ-Deklaration Ereignis-Behandler Methode

    #region Ereignisdatenklasse

    /// <summary>
    /// Stellt die Daten für das
    /// FehlerAufgetreten Ereignis bereit
    /// </summary>
    public class FehlerAufgetretenEventArgs
        : System.EventArgs
    {

        #region Fehlerursache (Ausnahme)

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private System.Exception _Ursache = null!;

        /// <summary>
        /// Ruft die Ausnahme ab, die den
        /// Fehler beschreibt
        /// </summary>
        public System.Exception Ursache => this._Ursache;

        #endregion Fehlerursache (Ausnahme)

        #region Konstruktor

        /// <summary>
        /// Initialisiert ein neues 
        /// FehlerAufgetretenEventArgs Objekt
        /// </summary>
        /// <param name="ursache">Ein System.Exception
        /// Objekt, das den Fehler beschreibt</param>
        public FehlerAufgetretenEventArgs(
                    System.Exception ursache)
        {
            this._Ursache = ursache;
        }

        #endregion Konstruktor
    }

    #endregion Ereignisdatenklasse
}
