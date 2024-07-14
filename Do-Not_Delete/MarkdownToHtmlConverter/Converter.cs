
using System.Linq;
using System.Text;

namespace MarkdownToHtmlConverter
{
    internal static class Converter
    {
        public static void Convert(string filePath, string outputPath)
        {
            File.WriteAllText(outputPath, HtmlBegin);

            try
            {
                var mdContent = File.ReadAllLines(filePath);
                var htmlContent = ProcessContent(mdContent);

                File.AppendAllText(outputPath, htmlContent);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"The file could not be read: {exception.Message}");
            }

            File.AppendAllText(outputPath, HtmlEnd);
        }

        private static string ProcessContent(string[] mdContent)
        {
            var stringBuilder = new StringBuilder();

            for (var lineCounter = 0; lineCounter < mdContent.Length; lineCounter++)
            {
                var line = mdContent[lineCounter];
                string html;

                if (line.StartsWith("#"))
                {
                    html = ProcessHeading(line);
                }
                else if (line.StartsWith("["))
                {
                    var tokens = line.Split(new char[] { '[', ']', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    html = $"<a href={tokens[1].Trim()}>{tokens[0].Trim()}</a>";
                }
                else if (line == "```csharp")
                {
                    var closingIndex = lineCounter + 1;
                    html = "<pre><code class=\"language-csharp\">";

                    while (mdContent[closingIndex] != "```")
                    {
                        html += mdContent[closingIndex].Replace("<", "&lt;").Replace(">", "&gt;") + "\n";
                        closingIndex++;
                    }

                    html += "</code></pre>";
                    lineCounter = closingIndex;
                }
                else
                {
                    line = ConvertToHtmlFriendlyFormat(line);
                    html = $"<p>{line}</p>";
                }

                stringBuilder.AppendLine(html);
            }
            
            return stringBuilder.ToString();
        }

        private static string ConvertToHtmlFriendlyFormat(string line)
        {
            // You **can** access individual elements of an **array** using the index, which is zero-based:
            // You <strong>can</strong> access individual elements of an <strong>array</strong> using the index, which is zero-based:
            line = line.Replace(" **", " <strong>");
            line = line.Replace("** ", "</strong> ");
            line = line.Replace("**:", "</strong>:");
            line = line.Replace("**,", "</strong>,");
            line = line.Replace("**.", "</strong>.");
            line = line.Replace(" `", " <span class=\"spec\">");
            line = line.Replace("` ", "</span> ");
            line = line.Replace("`:", "</span>:");
            line = line.Replace("`,", "</span>,");
            line = line.Replace("`.", "</span>.");

            return line;
        }

        private static string ProcessHeading(string line)
        {
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var hashtagsCount = tokens[0].Length;
            var content = tokens[1].Trim();

            return $"<h{hashtagsCount}>{content}</h{hashtagsCount}>";
        }

        private static string HtmlBegin =>
@"<html>
<head>
<script src=""prism.js""></script>
<link rel=""stylesheet"" href=""prism.css"" />
</head>
<body>
";

        private static string HtmlEnd =>
@"</body>
</html>";
    }
}
