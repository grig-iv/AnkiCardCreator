using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LongmanDictionary.Models.Pages;
using ReactiveUI;

namespace DesktopApp.Models;

public record SearchRequest : IObservable<Result<AbstractPage>>
{
    private readonly CancellationTokenSource _cts;

    public SearchRequest(
        string searchedWord,
        Task<Result<AbstractPage>> respond,
        CancellationTokenSource cts, 
        IScheduler scheduler)
    {
        _cts = cts;
        SearchedWord = searchedWord;
        Respond = respond.ToObservable().ObserveOn(scheduler);

        Respond.Subscribe(result =>
        {
            IsComplete = true;
            MaybePage = result.Match(Maybe.From, _ => Maybe<AbstractPage>.None);
        });
    }
    
    public string SearchedWord { get; }
    public IObservable<Result<AbstractPage>> Respond { get; }
    public bool IsComplete { get; private set; }
    public Maybe<AbstractPage> MaybePage { get; private set; }

    public void Cancel()
    {
        if (IsComplete)
            return;
        
        _cts.Cancel();
    }
    
    public IDisposable Subscribe(IObserver<Result<AbstractPage>> observer)
    {
        return Respond.Subscribe(observer);
    }
}