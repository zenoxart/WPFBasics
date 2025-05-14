using Zenox.Wpf.Core.Common.MVVM.FactoryInjection;

namespace Zenox.Wpf.Core.Common.Controller
{

    /// <summary>
    /// Stellt einen Dienst zum Schreiben
    /// und Lesen von Xml Daten einer Liste bereit.
    /// </summary>
    public class XmlController<T> : AppObject
    {
        /// <summary>
        /// Serialisiert die Daten in den
        /// angegebenen Pfad im Xml Format
        /// </summary>
        /// <param name="pfad">Der vollständige zu
        /// benutzende Dateiname</param>
        /// <param name="daten">Die Liste mit den
        /// zu speichernden Informationen</param>
        /// <exception cref="System.Exception">Tritt auf,
        /// wenn beim Serialisieren ein Fehler aufgetreten ist</exception>
        public void Speichern(string pfad, T daten)
        {
            using var Schreiber = new System.IO.StreamWriter(pfad);
            var Serialisierer = new System.Xml.Serialization
                .XmlSerializer(daten!.GetType());

            Serialisierer.Serialize(Schreiber, daten);
        }

        /// <summary>
        /// Gibt den deserialisierten Xml Inhalt
        /// aus der angegebenen Datei zurück
        /// </summary>
        /// <param name="pfad">Der vollständige zu
        /// benutzende Dateiname</param>
        /// <exception cref="System.Exception">Tritt auf,
        /// wenn beim Deserialisieren ein Fehler aufgetreten ist</exception>
        public T Lesen(string pfad)
        {
            using var Leser = new System.IO.StreamReader(pfad);
            var Serialisierer = new System.Xml.Serialization
                .XmlSerializer(typeof(T));

            return (T)Serialisierer.Deserialize(Leser)!;
        }
    }
}
