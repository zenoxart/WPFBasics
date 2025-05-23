﻿using System.Collections.Concurrent;
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
