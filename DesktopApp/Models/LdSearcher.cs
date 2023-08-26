using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using LongmanDictionary.Models.Pages;
using LongmanDictionary.Services;

namespace DesktopApp.Models;

public class LdSearcher
{
    private readonly IScheduler _scheduler;
    private readonly Subject<SearchRequest> _whenSearchRequested;
    private readonly SearchService _ldoceSearch;
    private SearchRequest? _lastRequest;

    public LdSearcher(IScheduler scheduler)
    {
        _scheduler = scheduler;
        _whenSearchRequested = new Subject<SearchRequest>();
        _ldoceSearch = new SearchService();
        
        WhenPageLoaded = _whenSearchRequested
            .SelectMany(r =>r.Respond)
            .Where(r => r.IsSuccess)
            .Select(r => r.GetValueOrDefault());
    }

    public IObservable<SearchRequest> WhenSearchRequested => _whenSearchRequested.AsObservable();
    public IObservable<AbstractPage> WhenPageLoaded { get; }

    public SearchRequest Search(string word)
    {
        if (_lastRequest?.SearchedWord == word && !_lastRequest.IsComplete)
            return _lastRequest;
        
        _lastRequest?.Cancel();
        
        var cts = new CancellationTokenSource();
        var respond = _ldoceSearch.SearchAsync(word, cts.Token);
        var request = new SearchRequest(word, respond, cts, _scheduler);

        _lastRequest = request;
        _whenSearchRequested.OnNext(request);
        
        return request;
    }
}