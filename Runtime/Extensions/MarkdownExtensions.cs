using System.Text;

namespace Hackbox
{
    public static class MarkdownExtensions
    {
        #region String Appending
        public static string AppendH1(this string text, string header) => $"{text}{MarkdownHelpers.H1(header)}";

        public static string AppendH2(this string text, string header) => $"{text}{MarkdownHelpers.H2(header)}";

        public static string AppendH3(this string text, string header) => $"{text}{MarkdownHelpers.H3(header)}";

        public static string AppendBold(this string text, string bold) => $"{text}{MarkdownHelpers.Bold(bold)}";

        public static string AppendItalics(this string text, string italics) => $"{text}{MarkdownHelpers.Italics(italics)}";

        public static string AppendStrikethrough(this string text, string strikethrough) => $"{text}{MarkdownHelpers.Strikethrough(strikethrough)}";

        public static string AppendBlockquote(this string text, string blockquote) => $"{text}{MarkdownHelpers.Blockquote(blockquote)}";

        public static string AppendOrderedList(this string text, string[] entries) => $"{text}{MarkdownHelpers.OrderedList(entries)}";

        public static string AppendUnorderedList(this string text, string[] entries) => $"{text}{MarkdownHelpers.UnorderedList(entries)}";

        public static string AppendCode(this string text, string code) => $"{text}{MarkdownHelpers.Code(code)}";

        public static string AppendCodeBlock(this string text, string code) => $"{text}{MarkdownHelpers.CodeBlock(code)}";

        public static string AppendLink(this string text, string link, string url) => $"{text}{MarkdownHelpers.Link(link, url)}";

        public static string AppendImage(this string text, string altText, string url) => $"{text}{MarkdownHelpers.Image(altText, url)}";

        public static string AppendMultiline(this string text, string[] lines) => $"{text}{MarkdownHelpers.Multiline(lines)}";

        public static string AppendTable(this string text, string[] headers, string[,] data) => $"{text}{MarkdownHelpers.Table(headers, data)}";

        public static string AppendSpan(this string text, (string property, string value)[] styling, string spanText) => $"{text}{MarkdownHelpers.Span(styling, spanText)}";

        public static string AppendDiv(this string text, (string property, string value)[] styling, string divText) => $"{text}{MarkdownHelpers.Span(styling, divText)}";

        public static string AppendLineBreak(this string text) => $"{text}{MarkdownHelpers.LineBreak}";

        public static string AppendHorizontalRule(this string text) => $"{text}{MarkdownHelpers.HorizontalRule}";
        #endregion

        #region StringBuilder Appending
        public static StringBuilder AppendH1(this StringBuilder text, string header) => text.Append(MarkdownHelpers.H1(header));

        public static StringBuilder AppendH2(this StringBuilder text, string header) => text.Append(MarkdownHelpers.H2(header));

        public static StringBuilder AppendH3(this StringBuilder text, string header) => text.Append(MarkdownHelpers.H3(header));

        public static StringBuilder AppendBold(this StringBuilder text, string bold) => text.Append(MarkdownHelpers.Bold(bold));

        public static StringBuilder AppendItalics(this StringBuilder text, string italics) => text.Append(MarkdownHelpers.Italics(italics));

        public static StringBuilder AppendStrikethrough(this StringBuilder text, string strikethrough) => text.Append(MarkdownHelpers.Strikethrough(strikethrough));

        public static StringBuilder AppendBlockquote(this StringBuilder text, string blockquote) => text.Append(MarkdownHelpers.Blockquote(blockquote));

        public static StringBuilder AppendOrderedList(this StringBuilder text, string[] entries) => text.Append(MarkdownHelpers.OrderedList(entries));

        public static StringBuilder AppendUnorderedList(this StringBuilder text, string[] entries) => text.Append(MarkdownHelpers.UnorderedList(entries));

        public static StringBuilder AppendCode(this StringBuilder text, string code) => text.Append(MarkdownHelpers.Code(code));

        public static StringBuilder AppendCodeBlock(this StringBuilder text, string code) => text.Append(MarkdownHelpers.CodeBlock(code));

        public static StringBuilder AppendLink(this StringBuilder text, string link, string url) => text.Append(MarkdownHelpers.Link(link, url));

        public static StringBuilder AppendImage(this StringBuilder text, string altText, string url) => text.Append(MarkdownHelpers.Image(altText, url));

        public static StringBuilder AppendMultiline(this StringBuilder text, string[] lines) => text.Append(MarkdownHelpers.Multiline(lines));

        public static StringBuilder AppendTable(this StringBuilder text, string[] headers, string[,] data) => text.Append(MarkdownHelpers.Table(headers, data));

        public static StringBuilder AppendSpan(this StringBuilder text, (string property, string value)[] styling, string spanText) => text.Append(MarkdownHelpers.Span(styling, spanText));

        public static StringBuilder AppendDiv(this StringBuilder text, (string property, string value)[] styling, string divText) => text.Append(MarkdownHelpers.Span(styling, divText));

        public static StringBuilder AppendLineBreak(this StringBuilder text) => text.Append(MarkdownHelpers.LineBreak);

        public static StringBuilder AppendHorizontalRule(this StringBuilder text) => text.Append(MarkdownHelpers.HorizontalRule);
        #endregion
    }
}
