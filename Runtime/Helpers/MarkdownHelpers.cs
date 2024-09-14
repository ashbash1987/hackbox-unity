using System.Linq;
using System.Text;

namespace Hackbox
{
    /// <summary>
    /// Helper class for generating Markdown formatted strings.
    /// </summary>
    public static class MarkdownHelpers
    {
        /// <summary>
        /// Generates a Markdown H1 header.
        /// </summary>
        /// <param name="text">The text to format as an H1 header.</param>
        /// <returns>A Markdown formatted H1 header string.</returns>
        public static string H1(string text) => $"# {text}\n";

        /// <summary>
        /// Generates a Markdown H2 header.
        /// </summary>
        /// <param name="text">The text to format as an H2 header.</param>
        /// <returns>A Markdown formatted H2 header string.</returns>
        public static string H2(string text) => $"## {text}\n";

        /// <summary>
        /// Generates a Markdown H3 header.
        /// </summary>
        /// <param name="text">The text to format as an H3 header.</param>
        /// <returns>A Markdown formatted H3 header string.</returns>
        public static string H3(string text) => $"### {text}\n";

        /// <summary>
        /// Formats text as bold in Markdown.
        /// </summary>
        /// <param name="text">The text to format as bold.</param>
        /// <returns>A Markdown formatted bold string.</returns>
        public static string Bold(string text) => $"**{text}**";

        /// <summary>
        /// Formats text as italics in Markdown.
        /// </summary>
        /// <param name="text">The text to format as italics.</param>
        /// <returns>A Markdown formatted italics string.</returns>
        public static string Italics(string text) => $"*{text}*";

        /// <summary>
        /// Formats text with strikethrough in Markdown.
        /// </summary>
        /// <param name="text">The text to format with strikethrough.</param>
        /// <returns>A Markdown formatted strikethrough string.</returns>
        public static string Strikethrough(string text) => $"~~{text}~~";

        /// <summary>
        /// Formats text as a blockquote in Markdown.
        /// </summary>
        /// <param name="text">The text to format as a blockquote.</param>
        /// <returns>A Markdown formatted blockquote string.</returns>
        public static string Blockquote(string text) => $"> {text}";

        /// <summary>
        /// Generates a Markdown ordered list.
        /// </summary>
        /// <param name="entries">The list entries.</param>
        /// <returns>A Markdown formatted ordered list string.</returns>
        public static string OrderedList(string[] entries) => string.Join("", entries.Select((x, i) => $"{i + 1}. {x} \n"));

        /// <summary>
        /// Generates a Markdown unordered list.
        /// </summary>
        /// <param name="entries">The list entries.</param>
        /// <returns>A Markdown formatted unordered list string.</returns>
        public static string UnorderedList(string[] entries) => string.Join("", entries.Select(x => $"- {x} \n"));

        /// <summary>
        /// Formats text as inline code in Markdown.
        /// </summary>
        /// <param name="text">The text to format as inline code.</param>
        /// <returns>A Markdown formatted inline code string.</returns>
        public static string Code(string text) => $"`{text}`";

        /// <summary>
        /// Formats text as a code block in Markdown.
        /// </summary>
        /// <param name="text">The text to format as a code block.</param>
        /// <returns>A Markdown formatted code block string.</returns>
        public static string CodeBlock(string text) => $"```\n{text}\n```\n";

        /// <summary>
        /// Generates a Markdown link.
        /// </summary>
        /// <param name="title">The link title.</param>
        /// <param name="link">The URL of the link.</param>
        /// <returns>A Markdown formatted link string.</returns>
        public static string Link(string title, string link) => $"[{title}]({link})";

        /// <summary>
        /// Generates a Markdown image.
        /// </summary>
        /// <param name="altText">The alt text for the image.</param>
        /// <param name="link">The URL of the image.</param>
        /// <returns>A Markdown formatted image string.</returns>
        public static string Image(string altText, string link) => $"![{altText}]({link})";

        /// <summary>
        /// Joins multiple lines into a single Markdown formatted string with line breaks.
        /// </summary>
        /// <param name="lines">The lines to join.</param>
        /// <returns>A Markdown formatted string with line breaks.</returns>
        public static string Multiline(params string[] lines) => string.Join(LineBreak, lines);

        /// <summary>
        /// Generates a Markdown table.
        /// </summary>
        /// <param name="headers">The table headers.</param>
        /// <param name="data">The table data.</param>
        /// <returns>A Markdown formatted table string.</returns>
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

        /// <summary>
        /// Formats text with custom styling using a span element.
        /// </summary>
        /// <param name="styling">The styling properties and values.</param>
        /// <param name="text">The text to format.</param>
        /// <returns>A Markdown formatted string with custom styling.</returns>
        public static string Span((string property, string value)[] styling, string text) => $"<span style=\"{string.Join(";", styling.Select(x => $"{x.property}: {x.value}"))}\">{text}</span>";

        /// <summary>
        /// Formats text with custom styling using a div element.
        /// </summary>
        /// <param name="styling">The styling properties and values.</param>
        /// <param name="text">The text to format.</param>
        /// <returns>A Markdown formatted string with custom styling.</returns>
        public static string Div((string property, string value)[] styling, string text) => $"<div style=\"{string.Join(";", styling.Select(x => $"{x.property}: {x.value}"))}\">{text}</div>";

        /// <summary>
        /// Gets a Markdown line break.
        /// </summary>
        public static string LineBreak => "<br>";

        /// <summary>
        /// Gets a Markdown horizontal rule.
        /// </summary>
        public static string HorizontalRule => "<hr>";
    }
}
