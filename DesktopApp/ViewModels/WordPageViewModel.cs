using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DesktopApp.Models;
using LongmanDictionary.Models;
using LongmanDictionary.Models.Pages;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DesktopApp.ViewModels;

public class WordPageViewModel : PageViewModel<WordPage>
{
    private readonly AnkiCardCreator _cardCreator;
    private readonly ISnackbarMessageQueue _snackbarMessageQueue;

    public WordPageViewModel(
        ISnackbarMessageQueue snackbarMessageQueue,
        LdSearcher ldSearcher,
        AnkiCardCreator cardCreator)
        : base(ldSearcher)
    {
        _cardCreator = cardCreator;
        _snackbarMessageQueue = snackbarMessageQueue;
        
        WhenPageLoaded.ToPropertyEx(this, x => x.WordPage);

        this
            .WhenAnyValue(x => x.SelectedExample)
            .Where(example => example is not null)
            .Select(example => WordPage!.Entries
                .SelectMany(entry => entry.Senses)
                .First(sense => sense.Examples.Contains(example)))
            .Subscribe(sense => SelectedSense = sense);

        this.WhenAnyValue(x => x.SelectedSense)
            .Where(sense => sense is not null)
            .Select(sense => WordPage!.Entries.First(entry => entry.Senses.Contains(sense)))
            .Subscribe(entry => SelectedEntry = entry);

        CreateCardCommand = ReactiveCommand.CreateFromTask(
            CreateCardExecuteAsync,
            this
                .WhenAnyValue(x => x.SelectedSense)
                .Select(sense => sense is not null)
        );
    }

    public ReactiveCommand<Unit, Unit> CreateCardCommand { get; }

    [ObservableAsProperty] public WordPage? WordPage { get; }
    
    [Reactive] public bool IsCardCreating { get; private set; }
    [Reactive] public Entry? SelectedEntry { get; set; }
    [Reactive] public Sense? SelectedSense { get; set; }
    [Reactive] public Example? SelectedExample { get; set; }

    private async Task CreateCardExecuteAsync()
    {
        IsCardCreating = true;

        var result = await Result.Try(async () =>
            await _cardCreator.CreateAsync(
                WordPage!.Title!,
                SelectedEntry!,
                SelectedSense!,
                SelectedExample)
        ).Bind(x => x);

        result.Match(
            () => _snackbarMessageQueue.Enqueue("Card was successfully created"),
            error => _snackbarMessageQueue.Enqueue($"Card creation failed. {error}")
        );

        IsCardCreating = false;
    }
}