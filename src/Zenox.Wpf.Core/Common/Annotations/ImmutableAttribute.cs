using System.Reflection;

namespace Zenox.Wpf.Core.Common.Annotations
{
    /// <summary>
    /// Markiert eine Klasse oder Struktur als unveränderlich (immutable).
    /// Alle Felder müssen readonly sein.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class ImmutableAttribute : Attribute
    {
        /// <summary>
        /// Überprüft, ob die angegebene Klasse oder Struktur die Anforderungen an Unveränderlichkeit erfüllt.
        /// </summary>
        /// <param name="type">Der zu überprüfende Typ.</param>
        /// <returns>True, wenn der Typ unveränderlich ist, andernfalls false.</returns>
        public static bool Validate(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Überprüfen, ob der Typ mit dem Attribut markiert ist
            if (!type.GetCustomAttributes(typeof(ImmutableAttribute), false).Any())
                throw new InvalidOperationException($"Der Typ {type.Name} ist nicht mit dem ImmutableAttribute markiert.");

            // Überprüfen, ob alle Felder readonly sind
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.IsInitOnly)
                {
                    return false; // Ein Feld ist nicht readonly
                }
            }

            return true; // Alle Felder sind readonly
        }
    }
}
