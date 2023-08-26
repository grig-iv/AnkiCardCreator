using System;
using System.Reactive.Linq;
using DesktopApp.Models;
using ReactiveUI;

namespace DesktopApp.ViewModels;

public abstract class PageViewModel<TPage> : ReactiveObject
{
    protected PageViewModel(LdSearcher searcher)
    {
        WhenPageLoaded = searcher.WhenPageLoaded.OfType<TPage>();
    }
    
    public IObservable<TPage> WhenPageLoaded { get; }
}