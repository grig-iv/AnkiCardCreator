using HtmlAgilityPack;
using LongmanDictionary.Models.Pages;
using Optional;

namespace LongmanDictionary.Services;

public class SearchService
{
    private const string SearchQuery = $"{LdUrls.Domain}/search/english/direct/?q={{0}}";
    private readonly HtmlWeb _web;
    private readonly PageFactory _pageFactory;

    public SearchService()
    {
        _web = new HtmlWeb();
        _pageFactory = new PageFactory();
    }

    public static string FormSearchQuery(string searchRequest)
    {
        return string.Format(SearchQuery, searchRequest);
    }

    public async Task<Option<AbstractPage, Exception>> SearchAsync(string word)
    {
        try
        {
            var queryUrl = FormSearchQuery(word);
            var htmlPage = await _web.LoadFromWebAsync(queryUrl);
            var page = _pageFactory.CreatePage(htmlPage);
            return page is null 
                ? Option.None<AbstractPage, Exception>(new Exception("Unknown page")) 
                : Option.Some<AbstractPage, Exception>(page);
        }
        catch (Exception e)
        {
            return Option.None<AbstractPage, Exception>(e);
        }
    }
}