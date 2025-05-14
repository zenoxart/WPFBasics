namespace Zenox.Wpf.Core.Common.MVVM.ModelSupport
{
    /// <summary>
    /// Definiert die Schnittstelle für alle Modellklassen.
    /// </summary>
    public interface IModelBase
    {
        /// <summary>
        /// Ruft die eindeutige ID des Modells ab oder legt diese fest.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Überprüft, ob das Modell gültig ist.
        /// </summary>
        /// <param name="cond">Eine optionale Bedingung, die die Gültigkeit definiert.</param>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn die Bedingung erfüllt ist, andernfalls <c>false</c>.
        /// </returns>
        bool IsValid(bool? cond);
    }

    /// <summary>
    /// Stellt die abstrakte Basisklasse für alle Modelle dar.
    /// Diese Klasse implementiert die grundlegenden Eigenschaften und Methoden, 
    /// die von allen abgeleiteten Modellen benötigt werden.
    /// </summary>
    public abstract record ModelBase : IModelBase
    {
        /// <summary>
        /// Ruft die eindeutige ID des Modells ab oder legt diese fest.
        /// Diese Eigenschaft ist erforderlich und muss einen Wert größer als 0 haben.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Überprüft, ob das Modell gültig ist.
        /// Die Gültigkeit wird basierend auf der übergebenen Bedingung überprüft.
        /// </summary>
        /// <param name="cond">Eine optionale Bedingung, die die Gültigkeit definiert.</param>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn die Bedingung erfüllt ist, andernfalls <c>false</c>.
        /// Wenn <paramref name="cond"/> <c>null</c> ist, wird eine <see cref="NotImplementedException"/> ausgelöst.
        /// </returns>
        /// <exception cref="NotImplementedException">Wird ausgelöst, wenn <paramref name="cond"/> <c>null</c> ist.</exception>
        public virtual bool IsValid(bool? cond = null)
        {
            if (cond == null)
            {
                throw new NotImplementedException();
            }
            return cond.Value;
        }
    }


}
