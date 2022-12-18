using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApp.Models;
using LongmanDictionary.Models.Pages;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace DesktopApp.ViewModels;

public class LdBodyViewModel : ObservableObject
{
    private readonly LdSearcher _searcher;
    private readonly AnkiCardCreator _cardCreator;
    private readonly ISnackbarMessageQueue _snackbarMessageQueue;

    public LdBodyViewModel(LdSearcher searcher, AnkiCardCreator cardCreator, ISnackbarMessageQueue snackbarMessageQueue)
    {
        _searcher = searcher;
        _cardCreator = cardCreator;
        _snackbarMessageQueue = snackbarMessageQueue;

        Page = searcher.WhenSearchRequested
            .SelectMany(r => r.Response)
            .CatchIgnore()
            .Select(ConvertPageToViewModel)
            .ToReadOnlyReactiveProperty();
    }

    public ReadOnlyReactiveProperty<PageViewModel?> Page { get; }

    public PageViewModel? ConvertPageToViewModel(AbstractPage page)
    {
        return page switch
        {
            WordNotFoundPage wordNotFoundPage => new WordNotFoundPageViewModel(wordNotFoundPage, _searcher),
            WordPage wordPage => new WordPageViewModel(wordPage, _cardCreator, _snackbarMessageQueue),
            _ => null,
        };
    }
}