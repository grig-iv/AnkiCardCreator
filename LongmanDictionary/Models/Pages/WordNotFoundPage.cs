using HtmlAgilityPack;

namespace LongmanDictionary.Models.Pages;

public record WordNotFoundPage : AbstractPage
{
    public WordNotFoundPage(HtmlDocument htmlPage)
    {
        SuggestionWords = htmlPage.DocumentNode
            .SelectNodes("//ul[@class='didyoumean']/li")
            .Select(node => node.InnerText)
            .Select(text => text.Trim())
            .ToList();
    }
    
    public IReadOnlyList<string> SuggestionWords { get; }
}