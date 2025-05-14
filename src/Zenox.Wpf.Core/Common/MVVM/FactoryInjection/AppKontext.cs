namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection
{
    public class AppKontext
    {
        #region Anwendungsobjekt-Fabrik

        /// <summary>
        /// Gibt ein initialisierten
        /// Anwendungsobjekt zurück
        /// </summary>
        /// <typeparam name="T">Der Datentyp
        /// des benötigten Anwendungsobjekts</typeparam>
        /// <returns>Ein Objekt, wo die Kontext
        /// Eigenschaft initialisiert ist und
        /// andere Vorbereitungsarbeiten 
        /// erledigt sind</returns>
        public T Produziere<T>()
            where T : AppObject, new()
        {
            T NeuesObjekt = new();

            // Das neue Anwendungsobjekt
            // vorbereiten..


            // (1) Diese Infrastruktur weitergeben
            NeuesObjekt.Kontext = this;

#if DEBUG
            // (2) Im Visual Studio Ausgabefenster
            //     im Fehlerfall einen Eintrag erstellen
            NeuesObjekt.FehlerAufgetreten
                += (sender, e)
                => System.Diagnostics.Debug.WriteLine(
                    $"==> FEHLER! {sender} Ausnahme \"{e.Ursache.Message}\"");

            // (3) Im Ausgabefenster eine Produktionsmeldung
            System.Diagnostics.Debug.WriteLine(
                $"==> {NeuesObjekt} produziert und initialisiert...");

#endif
            //Damit beim Protokoll keine Rekursion auftritt
            if (this._Log != null)
            {
                this.Log.Eintragen($"==> {NeuesObjekt} produziert und initialisiert...");
            }
            NeuesObjekt.FehlerAufgetreten
                += (sender, e)
                => this.Log.Eintragen(
                    $"==> FEHLER! {sender} Ausnahme \"{e.Ursache.Message}\"",
                    LogEintragTyp.Fehler);

            // Hier weitere Initalisierungen ergänzen

            return NeuesObjekt;
        }

        #endregion Anwendungsobjekt-Fabrik

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private LogManager _Log = null!;

        /// <summary>
        /// Ruft den Protokolldienst ab
        /// </summary>
        public LogManager Log
        {
            get
            {
                if (this._Log == null)
                {
                    this._Log = this.Produziere<LogManager>();
                }

                return this._Log;
            }
        }
    }
}
