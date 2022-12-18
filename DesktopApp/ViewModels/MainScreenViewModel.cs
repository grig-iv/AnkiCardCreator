using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopApp.Models;
using DesktopApp.Services;
using LongmanDictionary.Services;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;

namespace DesktopApp.ViewModels;

public class MainScreenViewModel : ObservableObject
{
    public MainScreenViewModel(LdSearcher searcher, IDialogService dialogService)
    {
        Word = searcher.WhenSearchRequested
            .Select(r => r.Word)
            .ToReactiveProperty(string.Empty);

        IsSearchBoxEmpty = Word
            .Select(string.IsNullOrEmpty)
            .ToReadOnlyReactiveProperty();

        IsSearching = new BusyNotifier();
        searcher.WhenSearchRequested
            .Subscribe(request =>
            {
                var searchingProcess = IsSearching.ProcessStart();
                request.Response
                    .CatchIgnore()
                    .Subscribe(_ => searchingProcess.Dispose());
            });

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

        ReachabilityChecker
            .CheckAsync(TimeSpan.FromSeconds(5))
            .ToObservable()
            .Where(isReachable => !isReachable)
            .ObserveOnUIDispatcher()
            .Subscribe(_ => dialogService.ShowWarningDialog($"Can't reach '{LdUrls.MainPage}'."));
    }

    public ReactiveCommand SearchCommand { get; }
    public ReactiveCommand OpenLdoceCommand { get; }

    public ReactiveProperty<string> Word { get; }
    public ReadOnlyReactiveProperty<bool> IsSearchBoxEmpty { get; }
    public BusyNotifier IsSearching { get; }
}