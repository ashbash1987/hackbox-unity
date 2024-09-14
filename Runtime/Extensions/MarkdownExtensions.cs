using System.Text;

namespace Hackbox
{
    /// <summary>
    /// Provides extension methods for appending Markdown formatting to strings and StringBuilders.
    /// </summary>
    public static class MarkdownExtensions
    {
        #region String Appending

        /// <summary>
        /// Appends a Markdown H1 header to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="header">The header text.</param>
        /// <returns>The text with the appended H1 header.</returns>
        public static string AppendH1(this string text, string header) => $"{text}{MarkdownHelpers.H1(header)}";

        /// <summary>
        /// Appends a Markdown H2 header to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="header">The header text.</param>
        /// <returns>The text with the appended H2 header.</returns>
        public static string AppendH2(this string text, string header) => $"{text}{MarkdownHelpers.H2(header)}";

        /// <summary>
        /// Appends a Markdown H3 header to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="header">The header text.</param>
        /// <returns>The text with the appended H3 header.</returns>
        public static string AppendH3(this string text, string header) => $"{text}{MarkdownHelpers.H3(header)}";

        /// <summary>
        /// Appends bold Markdown formatting to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="bold">The text to format as bold.</param>
        /// <returns>The text with the appended bold formatting.</returns>
        public static string AppendBold(this string text, string bold) => $"{text}{MarkdownHelpers.Bold(bold)}";

        /// <summary>
        /// Appends italics Markdown formatting to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="italics">The text to format as italics.</param>
        /// <returns>The text with the appended italics formatting.</returns>
        public static string AppendItalics(this string text, string italics) => $"{text}{MarkdownHelpers.Italics(italics)}";

        /// <summary>
        /// Appends strikethrough Markdown formatting to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="strikethrough">The text to format with strikethrough.</param>
        /// <returns>The text with the appended strikethrough formatting.</returns>
        public static string AppendStrikethrough(this string text, string strikethrough) => $"{text}{MarkdownHelpers.Strikethrough(strikethrough)}";

        /// <summary>
        /// Appends a Markdown blockquote to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="blockquote">The blockquote text.</param>
        /// <returns>The text with the appended blockquote.</returns>
        public static string AppendBlockquote(this string text, string blockquote) => $"{text}{MarkdownHelpers.Blockquote(blockquote)}";

        /// <summary>
        /// Appends a Markdown ordered list to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="entries">The list entries.</param>
        /// <returns>The text with the appended ordered list.</returns>
        public static string AppendOrderedList(this string text, string[] entries) => $"{text}{MarkdownHelpers.OrderedList(entries)}";

        /// <summary>
        /// Appends a Markdown unordered list to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="entries">The list entries.</param>
        /// <returns>The text with the appended unordered list.</returns>
        public static string AppendUnorderedList(this string text, string[] entries) => $"{text}{MarkdownHelpers.UnorderedList(entries)}";

        /// <summary>
        /// Appends inline code Markdown formatting to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="code">The code text.</param>
        /// <returns>The text with the appended inline code formatting.</returns>
        public static string AppendCode(this string text, string code) => $"{text}{MarkdownHelpers.Code(code)}";

        /// <summary>
        /// Appends a code block Markdown formatting to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="code">The code text.</param>
        /// <returns>The text with the appended code block formatting.</returns>
        public static string AppendCodeBlock(this string text, string code) => $"{text}{MarkdownHelpers.CodeBlock(code)}";

        /// <summary>
        /// Appends a Markdown link to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="link">The link text.</param>
        /// <param name="url">The URL of the link.</param>
        /// <returns>The text with the appended link.</returns>
        public static string AppendLink(this string text, string link, string url) => $"{text}{MarkdownHelpers.Link(link, url)}";

        /// <summary>
        /// Appends a Markdown image to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="altText">The alt text for the image.</param>
        /// <param name="url">The URL of the image.</param>
        /// <returns>The text with the appended image.</returns>
        public static string AppendImage(this string text, string altText, string url) => $"{text}{MarkdownHelpers.Image(altText, url)}";

        /// <summary>
        /// Appends multiple lines to the text with Markdown line breaks.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="lines">The lines to append.</param>
        /// <returns>The text with the appended lines.</returns>
        public static string AppendMultiline(this string text, string[] lines) => $"{text}{MarkdownHelpers.Multiline(lines)}";

        /// <summary>
        /// Appends a Markdown table to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="headers">The table headers.</param>
        /// <param name="data">The table data.</param>
        /// <returns>The text with the appended table.</returns>
        public static string AppendTable(this string text, string[] headers, string[,] data) => $"{text}{MarkdownHelpers.Table(headers, data)}";

        /// <summary>
        /// Appends custom styled text using a span element to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="styling">The styling properties and values.</param>
        /// <param name="spanText">The text to format.</param>
        /// <returns>The text with the appended custom styled span.</returns>
        public static string AppendSpan(this string text, (string property, string value)[] styling, string spanText) => $"{text}{MarkdownHelpers.Span(styling, spanText)}";

        /// <summary>
        /// Appends custom styled text using a div element to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="styling">The styling properties and values.</param>
        /// <param name="divText">The text to format.</param>
        /// <returns>The text with the appended custom styled div.</returns>
        public static string AppendDiv(this string text, (string property, string value)[] styling, string divText) => $"{text}{MarkdownHelpers.Span(styling, divText)}";

        /// <summary>
        /// Appends a Markdown line break to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <returns>The text with the appended line break.</returns>
        public static string AppendLineBreak(this string text) => $"{text}{MarkdownHelpers.LineBreak}";

        /// <summary>
        /// Appends a Markdown horizontal rule to the text.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <returns>The text with the appended horizontal rule.</returns>
        public static string AppendHorizontalRule(this string text) => $"{text}{MarkdownHelpers.HorizontalRule}";
        #endregion

        #region StringBuilder Appending
        /// <summary>
        /// Appends a Markdown H1 header to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="header">The header text.</param>
        /// <returns>The StringBuilder with the appended H1 header.</returns>
        public static StringBuilder AppendH1(this StringBuilder text, string header) => text.Append(MarkdownHelpers.H1(header));

        /// <summary>
        /// Appends a Markdown H2 header to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="header">The header text.</param>
        /// <returns>The StringBuilder with the appended H2 header.</returns>
        public static StringBuilder AppendH2(this StringBuilder text, string header) => text.Append(MarkdownHelpers.H2(header));

        /// <summary>
        /// Appends a Markdown H3 header to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="header">The header text.</param>
        /// <returns>The StringBuilder with the appended H3 header.</returns>
        public static StringBuilder AppendH3(this StringBuilder text, string header) => text.Append(MarkdownHelpers.H3(header));

        /// <summary>
        /// Appends bold Markdown formatting to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="bold">The text to format as bold.</param>
        /// <returns>The StringBuilder with the appended bold formatting.</returns>
        public static StringBuilder AppendBold(this StringBuilder text, string bold) => text.Append(MarkdownHelpers.Bold(bold));

        /// <summary>
        /// Appends italics Markdown formatting to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="italics">The text to format as italics.</param>
        /// <returns>The StringBuilder with the appended italics formatting.</returns>
        public static StringBuilder AppendItalics(this StringBuilder text, string italics) => text.Append(MarkdownHelpers.Italics(italics));

        /// <summary>
        /// Appends strikethrough Markdown formatting to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="strikethrough">The text to format with strikethrough.</param>
        /// <returns>The StringBuilder with the appended strikethrough formatting.</returns>
        public static StringBuilder AppendStrikethrough(this StringBuilder text, string strikethrough) => text.Append(MarkdownHelpers.Strikethrough(strikethrough));

        /// <summary>
        /// Appends a Markdown blockquote to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="blockquote">The blockquote text.</param>
        /// <returns>The StringBuilder with the appended blockquote.</returns>
        public static StringBuilder AppendBlockquote(this StringBuilder text, string blockquote) => text.Append(MarkdownHelpers.Blockquote(blockquote));

        /// <summary>
        /// Appends a Markdown ordered list to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="entries">The list entries.</param>
        /// <returns>The StringBuilder with the appended ordered list.</returns>
        public static StringBuilder AppendOrderedList(this StringBuilder text, string[] entries) => text.Append(MarkdownHelpers.OrderedList(entries));

        /// <summary>
        /// Appends a Markdown unordered list to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="entries">The list entries.</param>
        /// <returns>The StringBuilder with the appended unordered list.</returns>
        public static StringBuilder AppendUnorderedList(this StringBuilder text, string[] entries) => text.Append(MarkdownHelpers.UnorderedList(entries));

        /// <summary>
        /// Appends inline code Markdown formatting to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="code">The code text.</param>
        /// <returns>The StringBuilder with the appended inline code formatting.</returns>
        public static StringBuilder AppendCode(this StringBuilder text, string code) => text.Append(MarkdownHelpers.Code(code));

        /// <summary>
        /// Appends a code block Markdown formatting to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="code">The code text.</param>
        /// <returns>The StringBuilder with the appended code block formatting.</returns>
        public static StringBuilder AppendCodeBlock(this StringBuilder text, string code) => text.Append(MarkdownHelpers.CodeBlock(code));

        /// <summary>
        /// Appends a Markdown link to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="link">The link text.</param>
        /// <param name="url">The URL of the link.</param>
        /// <returns>The StringBuilder with the appended link.</returns>
        public static StringBuilder AppendLink(this StringBuilder text, string link, string url) => text.Append(MarkdownHelpers.Link(link, url));

        /// <summary>
        /// Appends a Markdown image to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="altText">The alt text for the image.</param>
        /// <param name="url">The URL of the image.</param>
        /// <returns>The StringBuilder with the appended image.</returns>
        public static StringBuilder AppendImage(this StringBuilder text, string altText, string url) => text.Append(MarkdownHelpers.Image(altText, url));

        /// <summary>
        /// Appends multiple lines to the StringBuilder with Markdown line breaks.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="lines">The lines to append.</param>
        /// <returns>The StringBuilder with the appended lines.</returns>
        public static StringBuilder AppendMultiline(this StringBuilder text, string[] lines) => text.Append(MarkdownHelpers.Multiline(lines));

        /// <summary>
        /// Appends a Markdown table to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="headers">The table headers.</param>
        /// <param name="data">The table data.</param>
        /// <returns>The StringBuilder with the appended table.</returns>
        public static StringBuilder AppendTable(this StringBuilder text, string[] headers, string[,] data) => text.Append(MarkdownHelpers.Table(headers, data));

        /// <summary>
        /// Appends custom styled text using a span element to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="styling">The styling properties and values.</param>
        /// <param name="spanText">The text to format.</param>
        /// <returns>The StringBuilder with the appended custom styled span.</returns>
        public static StringBuilder AppendSpan(this StringBuilder text, (string property, string value)[] styling, string spanText) => text.Append(MarkdownHelpers.Span(styling, spanText));

        /// <summary>
        /// Appends custom styled text using a div element to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <param name="styling">The styling properties and values.</param>
        /// <param name="divText">The text to format.</param>
        /// <returns>The StringBuilder with the appended custom styled div.</returns>
        public static StringBuilder AppendDiv(this StringBuilder text, (string property, string value)[] styling, string divText) => text.Append(MarkdownHelpers.Span(styling, divText));

        /// <summary>
        /// Appends a Markdown line break to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <returns>The StringBuilder with the appended line break.</returns>
        public static StringBuilder AppendLineBreak(this StringBuilder text) => text.Append(MarkdownHelpers.LineBreak);

        /// <summary>
        /// Appends a Markdown horizontal rule to the StringBuilder.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <returns>The StringBuilder with the appended horizontal rule.</returns>
        public static StringBuilder AppendHorizontalRule(this StringBuilder text) => text.Append(MarkdownHelpers.HorizontalRule);
        #endregion
    }
}
