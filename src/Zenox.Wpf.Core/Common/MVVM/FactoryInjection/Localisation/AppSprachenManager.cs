namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection.Localisation
{
    public class AppSprachenManager : AppObject
    {
        #region Datenhaltung

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private SprachenXmlController
            _Controller = null!;

        /// <summary>
        /// Ruft den Dienst zum Speichern
        /// und Lesen der Anwendungssprachen ab
        /// </summary>
        private SprachenXmlController Controller
        {
            get
            {
                if (_Controller == null)
                {
                    _Controller
                        = Kontext.Produziere<SprachenXmlController>();
                }

                return _Controller;
            }
        }

        #endregion Datenhaltung

        #region Bekannte Sprachen

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Sprachen _Liste = null!;

        /// <summary>
        /// Ruft die möglichen Anwendungssprachen ab
        /// </summary>
        /// <remarks>Der Inhalt wird über die
        /// lokalisierte Sprachen.xml Datei 
        /// in den Awendungsressourcen initialisiert.
        /// Sollte hier ein Problem sein, wird
        /// eine leere Liste bereitgestellt.
        /// Der Inhalt wird seit 20250327 
        /// alphabetisch sortiert</remarks>
        public Sprachen Liste
        {
            get
            {
                if (_Liste == null)
                {
                    try
                    {
                        //20250327 Die Daten sortiert liefern
                        /*
                        this._Liste = this.Controller
                            .HoleAusRessourcen();
                        */
                        _Liste = new Sprachen();
                        _Liste.AddRange(
                            from s in Controller.HoleAusRessourcen()
                            orderby s.Name
                            select s);
                    }
                    catch (Exception ex)
                    {
                        // Damit wir nicht wiederholt
                        // in das Problem laufen
                        _Liste = new Sprachen();

                        // HIER AUF KEINEN FALL
                        // EINE MessageBox !!!

                        OnFehlerAufgetreten(
                            new FehlerAufgetretenEventArgs(ex)
                            );
                    }
                }

                return _Liste;
            }
        }


        #endregion Bekannte Sprachen

        #region Aktuelle Anwendungssprache

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private Sprache
            _AktuelleSprache = null!;

        /// <summary>
        /// Ruft die aktuelle Anzeigesprache
        /// der Anwendung ab oder legt diese fest
        /// </summary>
        /// <remarks>Als Standard wird die Sprache
        /// (2Stellig) des Betriebssystems benutzt oder
        /// Englisch, falls keine Lokalisierung
        /// vorhanden ist</remarks>
        public Sprache AktuelleSprache
        {
            get
            {
                if (_AktuelleSprache == null)
                {
                    var OScode = System.Globalization
                        .CultureInfo
                            .CurrentUICulture
                                .TwoLetterISOLanguageName;

                    var OSSprache = Liste.Suchen(OScode);

                    // Falls nicht von uns unterstützt,
                    // Englisch benutzen
                    if (OSSprache == null)
                    {
                        OSSprache = Festlegen("en");
                    }

                    _AktuelleSprache = OSSprache;
                }

                return _AktuelleSprache;
            }
            set => _AktuelleSprache = value;
        }


        /// <summary>
        /// Stellt die CurrentUICulture auf die
        /// gewünschte Sprache ein und initialisiert
        /// den Sprachen-Dienst neu
        /// </summary>
        /// <param name="codeISO2">Der 2stellige Sprachcode,
        /// den Microsoft benutzt</param>
        /// <returns>Die Sprache, die aktuell benutzt wird</returns>
        /// <remarks>Sollte die gewünschte Sprache nicht
        /// unterstützt werden, wird Englisch eingestellt</remarks>
        public Sprache Festlegen(string codeISO2)
        {
            var NeueSprache = Liste.Suchen(codeISO2);
            if (NeueSprache == null)
            {
                NeueSprache = Liste.Suchen("en");
            }

            if (!System.Globalization.CultureInfo
                    .CurrentUICulture
                        .TwoLetterISOLanguageName
                            .Equals(
                                NeueSprache!.Code,
                                StringComparison.InvariantCultureIgnoreCase))
            {
                System.Globalization.CultureInfo
                    .CurrentUICulture
                        = new System.Globalization
                                .CultureInfo(NeueSprache!.Code);
                // Dafür sorgen, dass der Inhalt unserer
                // Liste ebenfalls lokalisiert wird, 
                // d.h. den Cache entfernen
                _Liste = null!;

                // Die aktuelle Sprache einstellen
                // aus der neuen lokalisierten Liste
                AktuelleSprache
                    = Liste.Suchen(NeueSprache.Code)!;
            }

            return NeueSprache;
        }



        #endregion Aktuelle Anwendungssprache
    }


    /// <summary>
    /// Stellt eine typsichere dynamische Liste
    /// für Anwendungssprachen bereit
    /// </summary>
    public class Sprachen
        : List<Sprache>
    {
        /// <summary>
        /// Gibt die Sprache mit dem
        /// gewünschten Code zurück
        /// </summary>
        /// <param name="code">Der 2stellige
        /// CultureInfo Code von Microsoft</param>
        /// <returns>Null, falls keine Sprache
        /// mit dem Code gefunden wurde</returns>
        /// <remarks>Die Methode ist case-insensitiv</remarks>
        public Sprache? Suchen(string code)
            => Find(
                s => s.Code.Equals(
                    code,
                    StringComparison.InvariantCultureIgnoreCase
                    )
                );
    }

    /// <summary>
    /// Beschreibt eine Anwendungssprache
    /// </summary>
    /// <remarks>Es handelt sich um
    /// ein Datentransferobjekt.</remarks>
    public class Sprache : AppObject
    {
        /// <summary>
        /// Ruft die lesbare Bezeichnung
        /// der Sprache ab oder legt diese fest
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ruft das von der Microsoft
        /// .Net CultureInfo benutzte Kürzel ab
        /// oder legt dieses fest
        /// </summary>
        /// <remarks>Das WIFI Firmenframework
        /// nutzt nur die Hauptsprache, keine
        /// Subkultur (2stelliger Code)</remarks>
        public string Code { get; set; } = string.Empty;

    }
}
