using WPFBasics.Common.MVVM.ModelSupport;

namespace UI.MVVM.Model
{
    /// <summary>
    /// Stellt ein Beispiel-Datenübertragungsobjekt (DTO) dar, das von <see cref="ModelBase"/> erbt.
    /// </summary>
    public record ExampleDTO : ModelBase
    {
        /// <summary>
        /// Ruft den Namen des Objekts ab oder legt diesen fest.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ruft die Beschreibung des Objekts ab oder legt diese fest.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Überprüft, ob das DTO gültig ist.
        /// Die Gültigkeit wird basierend auf der Basisklassenprüfung sowie den Eigenschaften <see cref="Name"/> und <see cref="Description"/> überprüft.
        /// </summary>
        /// <param name="cond">Eine optionale Bedingung, die die Gültigkeit definiert.</param>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn die Basisklassenprüfung erfolgreich ist und sowohl <see cref="Name"/> als auch <see cref="Description"/> leer sind, andernfalls <c>false</c>.
        /// </returns>
        public override bool IsValid(bool? cond = null)
        {
            return base.IsValid(
               Id > 0
            && string.IsNullOrEmpty(Name)
            && string.IsNullOrEmpty(Description));
        }
    }
}
