using CSharpFunctionalExtensions;
using HtmlAgilityPack;
using LongmanDictionary.Models.Pages;

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

    public async Task<Result<AbstractPage>> SearchAsync(
        string word,
        CancellationToken cancellationToken)
    {
        return await Result.Try(async () =>
        {
            var queryUrl = FormSearchQuery(word);
            var htmlPage = await _web.LoadFromWebAsync(queryUrl, cancellationToken);
            return _pageFactory.CreatePage(htmlPage);
        }).Bind(x => x);
    }
}