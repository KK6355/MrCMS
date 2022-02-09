using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MrCMS.Shortcodes
{
    public class ShortcodeParser : IShortcodeParser
    {
        private static readonly Regex ShortcodeMatcher =
            new Regex(@"\[([\w-_]+)([^\]]*)?\]?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex AttributeMatcher =
            new Regex(@"(\w+)\s*=\s*""?(\d+|\w+)""?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly IRenderShortcode _renderShortcode;

        public ShortcodeParser(IRenderShortcode renderShortcode)
        {
            _renderShortcode = renderShortcode;
        }

        public IHtmlContent Parse(IHtmlHelper htmlHelper, string current)
        {
            if (string.IsNullOrWhiteSpace(current))
            {
                return HtmlString.Empty;
            }

            current = ShortcodeMatcher.Replace(current, match =>
            {
                var tagName = match.Groups[1].Value;
                if (!_renderShortcode.CanRender(tagName))
                {
                    return string.Empty;
                }

                var matches = AttributeMatcher.Matches(HttpUtility.HtmlDecode( match.Groups[2].Value));

                var attributes = matches.ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value);

                using (var writer = new StringWriter())
                {
                    var content = _renderShortcode.Render(htmlHelper, tagName, attributes);
                    content.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                    return writer.ToString();
                }
            });

            return new HtmlString(current);
        }
    }
}