using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using CSharpFunctionalExtensions;
using DesktopApp.Models;
using LongmanDictionary.Services;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DesktopApp.ViewModels;

public class HeaderViewModel : ReactiveObject
{
    public HeaderViewModel(ISnackbarMessageQueue snackbarMessageQueue, LdSearcher ldSearcher)
    {
        ldSearcher.WhenSearchRequested
            .Select(r => r.SearchedWord)
            .Subscribe(w => Word = w);

        ldSearcher.WhenSearchRequested
            .SelectMany(request => Observable
                .Return(true)
                .Concat(request.Select(_ => false)))
            .ToPropertyEx(this, x => x.IsSearching);

        this
            .WhenAnyValue(x => x.Word)
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Throttle(TimeSpan.FromSeconds(1.5))
            .Select(NormalizeWord!)
            .Subscribe(world => ldSearcher.Search(world));

        OpenLdoceCommand = ReactiveCommand.Create(OpenLdoce);

        SearchCommand = ReactiveCommand.Create(
            () => ldSearcher.Search(NormalizeWord(Word!)),
            this.WhenAnyValue(x => x.Word).Select(w => !string.IsNullOrWhiteSpace(w))
        );

        SearchCommand
            .SelectMany(r => r.Respond)
            .Subscribe(r => r.TapError(
                error => snackbarMessageQueue.Enqueue($"Search failed: {error}")
            ));
    }

    public ReactiveCommand<Unit, SearchRequest> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenLdoceCommand { get; }

    [ObservableAsProperty] public bool IsSearching { get; }
    [Reactive] public string? Word { get; set; }

    private void OpenLdoce()
    {
        var url = string.IsNullOrWhiteSpace(Word)
            ? LdUrls.MainPage
            : SearchService.FormSearchQuery(NormalizeWord(Word));

        Process.Start("explorer", $"\"{url}\"");
    }

    private string NormalizeWord(string word)
    {
        return word.Trim().ToLowerInvariant();
    }
}