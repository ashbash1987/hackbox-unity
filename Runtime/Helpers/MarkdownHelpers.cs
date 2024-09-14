using System.Linq;
using System.Text;

namespace Hackbox
{
    public static class MarkdownHelpers
    {
        public static string H1(string text) => $"# {text}\n";

        public static string H2(string text) => $"## {text}\n";

        public static string H3(string text) => $"### {text}\n";

        public static string Bold(string text) => $"**{text}**";

        public static string Italics(string text) => $"*{text}*";

        public static string Strikethrough(string text) => $"~~{text}~~";

        public static string Blockquote(string text) => $"> {text}";

        public static string OrderedList(string[] entries) => string.Join("", entries.Select((x, i) => $"{i + 1}. {x} \n"));

        public static string UnorderedList(string[] entries) => string.Join("", entries.Select(x => $"- {x} \n"));

        public static string Code(string text) => $"`{text}`";

        public static string CodeBlock(string text) => $"```\n{text}\n```\n";

        public static string Link(string title, string link) => $"[{title}]({link})";

        public static string Image(string altText, string link) => $"![{altText}]({link})";

        public static string Table(string[] headers, string[,] data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"| {string.Join(" | ", headers)} |\n");
            sb.Append($"| {string.Join(" | ", headers.Select(_ => "---"))} |\n");

            int rowCount = data.GetLength(0);
            int columnCount = data.GetLength(1);
            for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
            {
                sb.Append($"| {string.Join(" | ", Enumerable.Range(0, columnCount).Select(x => data[rowIndex, x]))} |\n");
            }

            return sb.ToString();
        }

        public static string Span((string property, string value)[] styling, string text) => $"<span style=\"{string.Join(";", styling.Select(x => $"{x.property}: {x.value}"))}\">{text}</span>";

        public static string Div((string property, string value)[] styling, string text) => $"<div style=\"{string.Join(";", styling.Select(x => $"{x.property}: {x.value}"))}\">{text}</div>";

        public static string LineBreak => "<br>";

        public static string HorizontalRule => "<hr>";
    }
}
