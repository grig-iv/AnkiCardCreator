using HtmlAgilityPack;

namespace LongmanDictionary.Utils;

public static class HtmlNodeExtension
{
    public static string? InnerPrettyText(this HtmlNode? node)
    {
        return node?.InnerText.Trim();
    }
}