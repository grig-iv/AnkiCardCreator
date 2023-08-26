using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows;
using DesktopApp.Models;
using DesktopApp.Services;
using LongmanDictionary.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DesktopApp.ViewModels;

public class MainScreenViewModel : ReactiveObject
{
    public MainScreenViewModel(LdSearcher searcher)
    {
        searcher.WhenSearchRequested
            .Select(r => r.Word)
            .ToPropertyEx(this, x => x.Word);

        this
            .WhenAnyValue(x => Word)
            .Select(string.IsNullOrEmpty)
            .ToPropertyEx(this, x => x.IsSearchBoxEmpty);


            /*
        SearchCommand = new ReactiveCommand(Word.Select(w => !string.IsNullOrWhiteSpace(w)), false);
        SearchCommand
            .Select(_ => Word.Value)
            .Merge(Word.Throttle(TimeSpan.FromSeconds(1.5)))
            .Select(w => w.Trim().ToLower())
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .DistinctUntilChanged()
            .Select(searcher.Search)
            .SelectMany(r => r.Response)
            .Subscribe(
                _ => { },
                ex => MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error));

        OpenLdoceCommand = new ReactiveCommand();
        OpenLdoceCommand.Subscribe(() =>
        {
            var url = string.IsNullOrWhiteSpace(Word.Value)
                ? LdUrls.MainPage
                : SearchService.FormSearchQuery(Word.Value);
            Process.Start("explorer", $"\"{url}\"");
        });
        */
    }

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenLdoceCommand { get; }

    [ObservableAsProperty] public bool IsSearchBoxEmpty { get; }
    [ObservableAsProperty] public bool IsSearching { get; }
    [ObservableAsProperty] public string Word { get; set; }
}