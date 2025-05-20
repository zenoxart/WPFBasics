namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection
{
    public class AppObject : IAppObject
    {
        public AppKontext Kontext { get; set; } = null!;

        /// <summary>
        /// Wird ausgelöst, wenn ein Problem 
        /// aufgetreten ist
        /// </summary>
        /// <remarks>Die Ursache befindet sich
        /// in den Ereignisdaten</remarks>
        public event FehlerAufgetretenEventHandler? FehlerAufgetreten;

        /// <summary>
        /// Löst das Ereignis FehlerAufgetreten aus
        /// </summary>
        /// <param name="e">Ereignisdaten</param>
        public virtual void OnFehlerAufgetreten(
                        FehlerAufgetretenEventArgs e)
        {
            // Damit die Garbage Collection
            // das Objekt nicht entfernt, mit
            // einer Kopie arbeiten
            var BehandlerKopie = this.FehlerAufgetreten;

            BehandlerKopie?.Invoke(this, e);
        }


        void IAppObject.OnFehlerAufgetreten(FehlerAufgetretenEventArgs e)
        {
            OnFehlerAufgetreten(e);
        }

        #region Protokollierung ...
        protected virtual void StartMelden([System.Runtime.CompilerServices.CallerMemberName] string aufrufer = null!)
        {
            if (this.Kontext.Log.Stufe == LogStufe.Alles)
            {
                var Besitzer = (new System.Diagnostics.StackTrace(skipFrames: 1))
                        .GetFrame(0)!
                        .GetMethod()!
                        .DeclaringType!.FullName;
                this.Kontext.Log.Eintragen($"{Besitzer}.{aufrufer} startet...");
            }
        }
        protected virtual void EndeMelden([System.Runtime.CompilerServices.CallerMemberName] string aufrufer = null!)
        {
            if (this.Kontext.Log.Stufe == LogStufe.Alles)
            {
                var Besitzer = (new System.Diagnostics.StackTrace(skipFrames: 1))
                        .GetFrame(0)!
                        .GetMethod()!
                        .DeclaringType!.FullName;
                this.Kontext.Log.Eintragen($"{Besitzer}.{aufrufer} beendet.");
            }
        }


        void IAppObject.StartMelden(string aufrufer)
        {
            StartMelden(aufrufer);
        }

        void IAppObject.EndeMelden(string aufrufer)
        {
            EndeMelden(aufrufer);
        }
        #endregion Protokollierung ...
    }
}
