using System.Reflection;

namespace Zenox.Wpf.Core.Common.Annotations
{
    /// <summary>
    /// Markiert eine Klasse oder Struktur als unver�nderlich (immutable).
    /// Alle Felder m�ssen readonly sein.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class ImmutableAttribute : Attribute
    {
        /// <summary>
        /// �berpr�ft, ob die angegebene Klasse oder Struktur die Anforderungen an Unver�nderlichkeit erf�llt.
        /// </summary>
        /// <param name="type">Der zu �berpr�fende Typ.</param>
        /// <returns>True, wenn der Typ unver�nderlich ist, andernfalls false.</returns>
        public static bool Validate(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            // �berpr�fen, ob der Typ mit dem Attribut markiert ist
            if (!type.GetCustomAttributes(typeof(ImmutableAttribute), false).Any())
                throw new InvalidOperationException($"Der Typ {type.Name} ist nicht mit dem ImmutableAttribute markiert.");

            // �berpr�fen, ob alle Felder readonly sind
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
