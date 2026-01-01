using Markdig;

namespace crud_with_ai_ui.Extensions;

public static class MarkdownExtensions
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public static string ConvertToHtml(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        return Markdown.ToHtml(markdown, Pipeline);
    }
}
