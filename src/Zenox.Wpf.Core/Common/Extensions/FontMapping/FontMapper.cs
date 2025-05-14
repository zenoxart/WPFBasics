using System.Resources;
using System.Text.Json;

namespace Zenox.Wpf.Core.Common.Extensions.FontMapping
{
    /// <summary>
    /// Singleton, der Fluent System Icons Codepoints zu Glyph-Strings abbildet.
    /// </summary>
    public class FontMapper
    {
        private static readonly Lazy<FontMapper> _instance = new(() => new FontMapper());

        /// <summary>
        /// Singleton-Instanz des FontMapper.
        /// </summary>
        public static FontMapper Instance => _instance.Value;

        private readonly Dictionary<string, string> _glyphs = new();

        // Namespace und ResourceName ggf. anpassen!
        private static readonly ResourceManager ResourceManager = new(

            "Zenox.Wpf.Core.Assets.Fonts.FluentSystemIconsRegular",
            typeof(FontMapper).Assembly
        );

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="FontMapper"/> Klasse.
        /// </summary>
        public FontMapper()
        {
            // Initialisierung synchron beim ersten Zugriff
            FetchAndInitializeAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Versucht, das Glyph für den angegebenen Icon-Namen zu ermitteln.
        /// </summary>
        /// <param name="iconName">Der PascalCase-Icon-Key (z.B. "Home20Regular").</param>
        /// <param name="glyph">Das resultierende Unicode-Glyph-String.</param>
        /// <returns>True, wenn das Icon gefunden wurde, sonst false.</returns>
        public bool TryGetGlyph(string iconName, out string glyph)
            => _glyphs.TryGetValue(iconName, out glyph!);

        /// <summary>
        /// Gibt das Glyph für den angegebenen Icon-Namen zurück oder wirft eine Exception, falls nicht gefunden.
        /// </summary>
        public string GetGlyph(string iconName)
        {
            if (!TryGetGlyph(iconName, out var glyph))
                throw new KeyNotFoundException($"Icon '{iconName}' nicht in der Zuordnung gefunden.");
            return glyph;
        }

        private async Task FetchAndInitializeAsync()
        {
            var data = await FetchFontContentsAsync();
            Initialize(data);
        }

        /// <summary>
        /// Füllt das interne Glyph-Dictionary mit den Rohdaten.
        /// </summary>
        private void Initialize(IDictionary<string, long> glyphData)
        {
            _glyphs.Clear();
            foreach (var kv in glyphData)
                _glyphs[kv.Key] = char.ConvertFromUtf32((int)kv.Value);
        }

        /// <summary>
        /// Lädt und parst das JSON aus der eingebetteten Resource in eine Name→Codepoint-Map.
        /// </summary>
        private static async Task<IDictionary<string, long>> FetchFontContentsAsync()
        {
            // ResourceName ggf. anpassen!
            if (ResourceManager.GetObject("FluentSystemIcons-Json") is not byte[] jsonContentBytes || jsonContentBytes.Length == 0)
                throw new InvalidOperationException("Fehler beim Laden des JSON-Inhalts aus der Resource-Datei.");

            // Simuliert asynchrones Verhalten, um CS1998 zu vermeiden
            //await Task.Yield();

            var jsonContent = System.Text.Encoding.UTF8.GetString(jsonContentBytes);

            var rawData = JsonSerializer.Deserialize<Dictionary<string, long>>(jsonContent)
                          ?? throw new InvalidOperationException("Fehler beim Parsen des JSON-Inhalts.");

            return rawData
                .OrderBy(kv => kv.Value)
                .ToDictionary(
                    kv => FormatIconName(kv.Key),
                    kv => kv.Value
                );
        }

        /// <summary>
        /// Wandelt Repository-Keys wie "ic_fluent_home_regular" in PascalCase-Namen um.
        /// </summary>
        private static string FormatIconName(string raw)
        {
            var trimmed = raw
                .Replace("ic_fluent_", string.Empty)
                .Replace("_regular", string.Empty)
                .Replace("_filled", string.Empty);

            return string.Concat(
                trimmed.Split('_', StringSplitOptions.RemoveEmptyEntries)
                       .Select(part => char.ToUpper(part[0]) + part.Substring(1))
            );
        }
    }
}
