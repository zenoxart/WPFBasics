using System.Resources;
using System.Text.Json;

namespace WPFBasics.Common.Extensions.FontMapping
{
    /// <summary>
    /// Singleton that fetches and maps Fluent System Icons code points to glyph strings.
    /// </summary>
    public class FontMapper
    {
        private static readonly Lazy<FontMapper> _instance = new(() => new FontMapper());
        public static FontMapper Instance => _instance.Value;

        private readonly Dictionary<string, string> _glyphs = new();

        private static readonly ResourceManager ResourceManager = new(
            "WPFBasics.Assets.Fonts.FluentSystemIconsRegular",
            typeof(FontMapper).Assembly
        );

        public FontMapper()
        {
            // Initialize synchronously on first access
            FetchAndInitializeAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Attempts to retrieve the glyph for the given icon name.
        /// </summary>
        /// <param name="iconName">The PascalCase icon key (e.g. "Home20Regular").</param>
        /// <param name="glyph">The resulting Unicode glyph string.</param>
        /// <returns>True if the icon was found; otherwise false.</returns>
        public bool TryGetGlyph(string iconName, out string glyph)
            => _glyphs.TryGetValue(iconName, out glyph!);

        /// <summary>
        /// Gets the glyph for the given icon name, or throws if not found.
        /// </summary>
        public string GetGlyph(string iconName)
        {
            if (!TryGetGlyph(iconName, out var glyph))
                throw new KeyNotFoundException($"Icon '{iconName}' not found in mapping.");
            return glyph;
        }

        private async Task FetchAndInitializeAsync()
        {
            var data = await FetchFontContentsAsync();
            Initialize(data);
        }

        /// <summary>
        /// Populates the internal glyph dictionary from raw mapping data.
        /// </summary>
        private void Initialize(IDictionary<string, long> glyphData)
        {
            _glyphs.Clear();
            foreach (var kv in glyphData)
                _glyphs[kv.Key] = char.ConvertFromUtf32((int)kv.Value);
        }

        /// <summary>
        /// Loads and parses the JSON from the embedded resource into a name→codepoint map.
        /// </summary>
        private static async Task<IDictionary<string, long>> FetchFontContentsAsync()
        {
            // Retrieve the JSON content from the .resx file as a byte array
            if (ResourceManager.GetObject("FluentSystemIcons-Json") is not byte[] jsonContentBytes || jsonContentBytes.Length == 0)
                throw new InvalidOperationException("Failed to load the JSON content from the resource file.");

            // Convert the byte array to a string
            var jsonContent = System.Text.Encoding.UTF8.GetString(jsonContentBytes);

            // Deserialize the JSON into a dictionary
            var rawData = JsonSerializer.Deserialize<Dictionary<string, long>>(jsonContent)
                          ?? throw new InvalidOperationException("Failed to parse the JSON content.");

            // Transform the keys into PascalCase and return the dictionary
            return rawData
                .OrderBy(kv => kv.Value) // Optional: Order by Unicode value
                .ToDictionary(
                    kv => FormatIconName(kv.Key), // Convert keys to PascalCase
                    kv => kv.Value // Keep the Unicode values as-is
                );
        }

        /// <summary>
        /// Converts repository keys like "ic_fluent_home_regular" to PascalCase names.
        /// </summary>
        private static string FormatIconName(string raw)
        {
            // Remove prefixes and suffixes
            var trimmed = raw
                .Replace("ic_fluent_", string.Empty)
                .Replace("_regular", string.Empty)
                .Replace("_filled", string.Empty);

            // Convert to PascalCase
            return string.Concat(
                trimmed.Split('_', StringSplitOptions.RemoveEmptyEntries)
                       .Select(part => char.ToUpper(part[0]) + part.Substring(1))
            );
        }
    }
}
