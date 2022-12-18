using HtmlAgilityPack;
using LongmanDictionary.Models.Pages;

namespace LongmanDictionary.Services;

internal class PageFactory
{
    public AbstractPage? CreatePage(HtmlDocument htmlPage)
    {
        var title = htmlPage.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
        
        if (title.Contains("Suggestions for", StringComparison.OrdinalIgnoreCase))
        {
            return new WordNotFoundPage(htmlPage);
        }
        if (title.Contains("meaning of", StringComparison.OrdinalIgnoreCase))
        {
            return new WordPage(htmlPage);
        }

        return null;
    }
}