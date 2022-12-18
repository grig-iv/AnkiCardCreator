using HtmlAgilityPack;
using LongmanDictionary.Utils;

namespace LongmanDictionary.Models.Pages;

public record WordPage : AbstractPage
{
    public WordPage(HtmlDocument htmlPage)
    {
        Title = htmlPage.DocumentNode.SelectSingleNode("//h1[@class='pagetitle']").InnerPrettyText();
        Entries = htmlPage.DocumentNode
            .SelectNodes("//div[@class='dictionary']/span[@class='dictentry']")
            ?.TakeWhile(node => !node.FirstChild.InnerText.Contains("From Longman Business Dictionary"))
            .Select(node => new Entry(node))
            .ToList() ?? new List<Entry>();
    }
    
    public string? Title { get; }
    public IReadOnlyList<Entry> Entries { get; }
}