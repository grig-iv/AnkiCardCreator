using System;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LongmanDictionary.Models.Pages;

namespace DesktopApp.Models;

public record SearchRequest : IObservable<Result<AbstractPage>>
{
    private readonly CancellationTokenSource _cts;

    public SearchRequest(
        string searchedWord,
        Task<Result<AbstractPage>> respond,
        CancellationTokenSource cts)
    {
        _cts = cts;
        SearchedWord = searchedWord;
        Respond = respond.ToObservable();
    }
    
    public string SearchedWord { get; }
    public IObservable<Result<AbstractPage>> Respond { get; }

    public void Cancel()
    {
        _cts.Cancel();
    }
    
    public IDisposable Subscribe(IObserver<Result<AbstractPage>> observer)
    {
        return Respond.Subscribe(observer);
    }
}