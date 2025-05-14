using Zenox.Wpf.Core.Common.Controller;

namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection.Localisation
{

    /// <summary>
    /// Stellt einen Dienst zum Lesen
    /// und Schreiben von Anwendungdaten
    /// im Xml Format bereit
    /// </summary>
    /// <remarks>Die Standardsprachen werden
    /// aus der Anwendungsressourcen geholt</remarks>
    internal class SprachenXmlController : XmlController<Sprachen>
    {

        #region Standardsprachen aus Ressourcen

        /// <summary>
        /// Gibt die Liste mit den Sprachen
        /// aus der lokalisierten Sprachen.xml
        /// Datei aus den Ressourcen zurück
        /// </summary>
        /// <exception cref="System.Exception">Tritt auf,
        /// wenn beim Lesen der Xml Datei ein Problem war</exception>
        /// <remarks>Es wird davon ausgegangen, dass
        /// das 1. Byte die UTF8 Signatur ist</remarks>
        public virtual Sprachen HoleAusRessourcen()
        {
            var Liste = new Sprachen();

            // Xml Text aus den Ressourcen
            var xmlText = Zenox.Wpf.Core.Properties.Resources.Localisation;


            // Zum Parsen in ein .Net XmlDocument
            var Xml = new System.Xml.XmlDocument();
            Xml.LoadXml(xmlText);

            // Für alle Elemente im Wurzel Element
            foreach (System.Xml.XmlElement e in Xml.DocumentElement!.ChildNodes)
            {
                // Ein Sprache Objekt mit den Attributen
                // initialisieren und in das Ergebnis
                // einfüllen ("Mappen")
                Liste.Add(
                    new Sprache
                    {
                        Code = e.GetAttribute("code"),
                        Name = e.GetAttribute("name")
                    });
            }

            return Liste;
        }

        #endregion Standardsprachen aus Ressourcen

    }
}
