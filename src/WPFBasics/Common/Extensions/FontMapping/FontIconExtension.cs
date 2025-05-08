using System.Windows.Markup;

namespace WPFBasics.Common.Extensions.FontMapping
{
    [MarkupExtensionReturnType(typeof(string))]
    public class FontIconExtension : MarkupExtension
    {
        /// <summary>
        /// The name of the icon, e.g. "Home16Regular".
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        /// The FontMapper instance used to resolve glyphs.
        /// </summary>
        public FontMapper FontMapper { get; set; } = new FontMapper();

        public FontIconExtension()
        {
        }

        public FontIconExtension(string iconName) : this()
        {
            IconName = iconName;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(IconName))
                return string.Empty;

            // Use the provided FontMapper instance to fetch the glyph
            if (FontMapper.TryGetGlyph(IconName, out var glyph))
            {
                return glyph;
            }

            // Return an empty string if the glyph is not found
            return string.Empty;
        }
    }
}
