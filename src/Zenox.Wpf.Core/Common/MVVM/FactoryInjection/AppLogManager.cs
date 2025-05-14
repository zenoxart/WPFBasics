namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection
{
    /// <summary>
    /// Beschreibt, welche Einträge
    /// protokolliert werden sollen
    /// </summary>
    public enum LogStufe
    {
        /// <summary>
        /// Schaltet die Protokollierung ab
        /// </summary>
        Aus = -1,
        /// <summary>
        /// Hinterlegt alle Protokolleinträge
        /// </summary>
        Alles = 1,
        /// <summary>
        /// Hier werden keine normalen
        /// Einträge protokolliert
        /// </summary>
        NurWarnungenUndFehler = 2,
        /// <summary>
        /// Hier werden keine normalen
        /// Einträge und Warnung protokolliert
        /// </summary>
        NurFehler = 3
    }

    /// <summary>
    /// Stellt einen Dienst zum
    /// Verwalten des Anwendungsprotolls bereit
    /// </summary>
    public class LogManager : AppObject
    {
        #region Zum Ein- und Abschalten

        /// <summary>
        /// Ruft die Information über 
        /// die Intensität der Protokollierung
        /// ab oder legt diese fest
        /// </summary>
        /// <remarks>Standard: Protokollierung aus</remarks>
        public LogStufe Stufe { get; set; } = LogStufe.Aus;

        #endregion Zum Ein- und Abschalten

        #region Für die Daten ...

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private LogEinträge _Liste = null!;

        /// <summary>
        /// Ruft die Einträge des Protokolls ab
        /// </summary>
        public LogEinträge Liste
        {
            get
            {
                if (this._Liste == null)
                {
                    this._Liste = new LogEinträge();
                }

                return this._Liste;
            }
        }

        #endregion Für die Daten ...

        #region Zum Hinzufügen von Einträgen ...

        /// <summary>
        /// Hinterlegt die Daten im Protokoll
        /// </summary>
        /// <param name="logEintrag">Ein LogEintrag Objekt,
        /// das die neue Zeile beschreibt</param>
        /// <remarks>Diese Methode führt keine Arbeiten durch,
        /// wenn die Protokollstufe auf Aus festgelegt ist</remarks>
        public void Eintragen(LogEintrag logEintrag)
        {
            if (this.Stufe != LogStufe.Aus)
            {
                if (this.Stufe == LogStufe.NurWarnungenUndFehler
                    && (logEintrag.Typ == LogEintragTyp.Warnung
                        || logEintrag.Typ == LogEintragTyp.Fehler))
                {
                    this.Liste.Add(logEintrag);
                }
                else if (this.Stufe == LogStufe.NurFehler
                    && logEintrag.Typ == LogEintragTyp.Fehler)
                {
                    this.Liste.Add(logEintrag);
                }
                else
                {
                    this.Liste.Add(logEintrag);
                }

                if (logEintrag.Typ == LogEintragTyp.Fehler)
                {
                    this.EnthältFehler = true;
                }
            }
        }

        /// <summary>
        /// Erstellt einen normalen Protokolleintrag
        /// </summary>
        /// <param name="text">Der Hinweis,
        /// der als normaler Eintrag hinzugefügt werden soll</param>
        public void Eintragen(string text)
        {
            this.Eintragen(
                new LogEintrag
                {
                    Text = text,
                    Typ = LogEintragTyp.Normal
                });
        }

        /// <summary>
        /// Erstellt einen Protokolleintrag
        /// </summary>
        /// <param name="text">Der Hinweis,
        /// der als Eintrag hinzugefügt werden soll</param>
        /// <param name="typ">Gibt die Eintragsvariante an</param>
        public void Eintragen(string text, LogEintragTyp typ)
        {
            this.Eintragen(
                new LogEintrag
                {
                    Text = text,
                    Typ = typ
                });
        }

        #endregion Zum Hinzufügen von Einträgen ...

        #region Enthält Fehler ...

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private bool _EnthältFehler = false;

        /// <summary>
        /// Ruft True ab, wenn im Protokoll
        /// Fehlereinträge enthalten sind
        /// </summary>
        public bool EnthältFehler
        {
            get => this._EnthältFehler;
            set
            {
                if (this._EnthältFehler != value)
                {
                    this._EnthältFehler = value;
                    this.OnEnthältFehlerGeändert(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Wird ausgelöst, wenn sich EnthältFehler geändert hat
        /// </summary>
        public event EventHandler EntähltFehlerGeändert = null!;

        /// <summary>
        /// Löst das Ereignis EnthältFehler aus
        /// </summary>
        /// <param name="e">Ereignisdaten</param>
        protected virtual void OnEnthältFehlerGeändert(EventArgs e)
        {
            var EnthältFehlerKopie = this.EntähltFehlerGeändert;
            EnthältFehlerKopie?.Invoke(this, e);
        }

        /// <summary>
        /// Setzt EnthältFehler zurück auf False
        /// </summary>
        public void FehlerBestätigen()
        {
            this.EnthältFehler = false;
        }

        #endregion Enthält Fehler ...
    }

    /// <summary>
    /// Beschreibt die Ursache
    /// eines Protokolleintrags
    /// </summary>
    public enum LogEintragTyp
    {
        /// <summary>
        /// Kennzeichnet Hinweise
        /// </summary>
        Normal,
        /// <summary>
        /// Kennzeichnet Hinweise,
        /// denen Beachtung geschenkt werden soll
        /// </summary>
        Warnung,
        /// <summary>
        /// Kennzeichnet Einträge, die
        /// wegen einer Ausnahme erstellt wurden
        /// </summary>
        Fehler
    }

    /// <summary>
    /// Stellt eine typsichere Auflistung
    /// für Protokolleinträge bereit
    /// </summary>
    /// <remarks>Die Liste ist für die
    /// WPF Datenbindung vorbereitet</remarks>
    public class LogEinträge : System.Collections.ObjectModel
        .ObservableCollection<LogEintrag>
    {

    }

    /// <summary>
    /// Beschreibt einen 
    /// Anwendungsprotokolleintrag
    /// </summary>
    public class LogEintrag
    {
        /// <summary>
        /// Ruft das Datum und die Uhrzeit
        /// beim Erzeugen dieses Eintrags ab
        /// </summary>
        public System.DateTime Zeitpunkt { get; } = System.DateTime.Now;

        /// <summary>
        /// Ruft die Beschreibung dieses
        /// Protokolleintrags ab oder 
        /// legt diese fest
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Ruft die Ursache des Protokolleintrags
        /// ab oder legt diese fest
        /// </summary>
        /// <remarks>Standard Normal</remarks>
        public LogEintragTyp Typ { get; set; } = LogEintragTyp.Normal;
    }
}
