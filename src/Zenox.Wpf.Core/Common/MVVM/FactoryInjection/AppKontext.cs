using System.Collections.Concurrent;
using Zenox.Wpf.Core.Common.MVVM.FactoryInjection.Localisation;

namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection
{
    public class AppKontext
    {
        // Thread-sichere Dictionary für generische Manager-Instanzen
        private readonly ConcurrentDictionary<Type, AppObject> _managerCache = new();

        /// <summary>
        /// Gibt einen Manager generisch zurück und erzeugt ihn bei Bedarf.
        /// </summary>
        /// <typeparam name="T">Der Typ des Managers</typeparam>
        /// <returns>Die Instanz des Managers</returns>
        public virtual T GetManager<T>() where T : AppObject, new()
        {
            return (T)_managerCache.GetOrAdd(typeof(T), _ =>
            {
                return this.Produziere<T>();
            });
        }

        #region Anwendungsobjekt-Fabrik

        public virtual T Produziere<T>()
            where T : AppObject, new()
        {
            T NeuesObjekt = new();

            NeuesObjekt.Kontext = this;

#if DEBUG
            NeuesObjekt.FehlerAufgetreten
                += (sender, e)
                => System.Diagnostics.Debug.WriteLine(
                    $"==> FEHLER! {sender} Ausnahme \"{e.Ursache.Message}\"");

            System.Diagnostics.Debug.WriteLine(
                $"==> {NeuesObjekt} produziert und initialisiert...");
#endif
            if (this._log != null)
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
        // Schritt-für-Schritt-Plan (Pseudocode):
        // 1. Überprüfe, ob param null oder leer ist. Wenn ja, verwende Standardkonstruktor wie bisher.
        // 2. Suche einen passenden Konstruktor für T anhand der Typen in param.
        // 3. Erzeuge eine Instanz von T mit Activator.CreateInstance und übergebe param.
        // 4. Setze Kontext und hänge Fehlerbehandlung wie gehabt an.
        // 5. Rückgabe des erzeugten Objekts.

        public virtual T Produziere<T>(object[] param)
            where T : AppObject
        {
            T NeuesObjekt;

            if (param == null || param.Length == 0)
            {
                NeuesObjekt = Activator.CreateInstance<T>();
            }
            else
            {
                var paramTypes = param.Select(p => p?.GetType() ?? typeof(object)).ToArray();
                var ctor = typeof(T).GetConstructor(paramTypes);
                if (ctor == null)
                {
                    throw new ArgumentException($"Kein passender Konstruktor für Typ {typeof(T).Name} mit den angegebenen Parametern gefunden.");
                }
                NeuesObjekt = (T)ctor.Invoke(param);
            }

            NeuesObjekt.Kontext = this;

#if DEBUG
            NeuesObjekt.FehlerAufgetreten
                += (sender, e)
                => System.Diagnostics.Debug.WriteLine(
                    $"==> FEHLER! {sender} Ausnahme \"{e.Ursache.Message}\"");

            System.Diagnostics.Debug.WriteLine(
                $"==> {NeuesObjekt} produziert und initialisiert...");
#endif
            if (this._log != null)
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



        private AppLogManager _log;
        /// <summary>
        /// Ruft den Anwendungsprotokoll-Manager ab, der für die Verwaltung und den Zugriff auf protokollbezogene Funktionalitäten verwendet wird.
        /// </summary>
        public AppLogManager Log
        {
            get
            {
                this._log = this.Produziere<AppLogManager>();
                return _log;
            }
        }


        /// <summary>
        /// Ruft den Manager ab, der für die Verwaltung der Anwendungssprachen zuständig ist.
        /// </summary>
        public AppSprachenManager Sprachen => GetManager<AppSprachenManager>();

        #endregion Anwendungsobjekt-Fabrik
    }
}
