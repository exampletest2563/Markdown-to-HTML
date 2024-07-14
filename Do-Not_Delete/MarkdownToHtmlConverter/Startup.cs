namespace MarkdownToHtmlConverter
{
    internal class Startup
    {
        static void Main()
        {
            var inputFile = "../../../file.md";
            var outputFile = "../../../output.html";
            Converter.Convert(inputFile, outputFile);
        }
    }
}
