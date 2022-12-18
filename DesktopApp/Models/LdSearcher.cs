using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Windows;
using LongmanDictionary.Models.Pages;
using LongmanDictionary.Services;
using Reactive.Bindings;

namespace DesktopApp.Models;

public class LdSearcher
{
    private readonly Subject<SearchRequest> _searchRequested;
    private readonly SearchService _ldoceSearch;

    public LdSearcher()
    {
        _searchRequested = new Subject<SearchRequest>();
        _ldoceSearch = new SearchService();
    }

    public IObservable<SearchRequest> WhenSearchRequested => _searchRequested.AsObservable();

    public SearchRequest Search(string word)
    {
        var response = _ldoceSearch
            .SearchAsync(word)
            .ToObservable()
            .SelectMany(res => res.Match(
                Observable.Return,
                Observable.Throw<AbstractPage>));

        var request = new SearchRequest(word, response);
        _searchRequested.OnNext(request);
        return request;
    }
}

public record SearchRequest(string Word, IObservable<AbstractPage> Response);
