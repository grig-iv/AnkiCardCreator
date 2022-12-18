using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;
using DesktopApp.Models;
using LongmanDictionary.Models;
using LongmanDictionary.Models.Pages;
using MaterialDesignThemes.Wpf;
using Optional;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;

namespace DesktopApp.ViewModels;

public class WordPageViewModel : PageViewModel
{
    private readonly AnkiCardCreator _cardCreator;
    private readonly ISnackbarMessageQueue _snackbarMessageQueue;

    public WordPageViewModel(WordPage wordPage, AnkiCardCreator cardCreator, ISnackbarMessageQueue snackbarMessageQueue)
    {
        _cardCreator = cardCreator;
        _snackbarMessageQueue = snackbarMessageQueue;

        WordPage = wordPage;

        IsCardCreating = new BooleanNotifier();

        SelectedExample = new ReactiveProperty<Example?>();

        SelectedSense = new ReactiveProperty<Sense?>(
            SelectedExample
                .Where(example => example is not null)
                .Select(example => WordPage.Entries
                    .SelectMany(entry => entry.Senses)
                    .First(sense => sense.Examples.Contains(example)))
        );

        SelectedEntry = new ReactiveProperty<Entry?>(
            SelectedSense
                .Where(sense => sense is not null)
                .Select(sense => wordPage.Entries.First(entry => entry.Senses.Contains(sense)))
        );

        CreateCardCommand = new ReactiveCommand(SelectedSense.Select(e => e is not null));
        CreateCardCommand.Subscribe(CreateCardExecute);
    }

    public ReactiveCommand CreateCardCommand { get; }

    public BooleanNotifier IsCardCreating { get; }

    public ReactiveProperty<Entry?> SelectedEntry { get; }
    public ReactiveProperty<Sense?> SelectedSense { get; }
    public ReactiveProperty<Example?> SelectedExample { get; }

    public WordPage WordPage { get; }

    private void CreateCardExecute()
    {
        IsCardCreating.TurnOn();

        var creationObs = _cardCreator
            .CreateAsync(
                WordPage.Title!,
                SelectedEntry.Value!,
                SelectedSense.Value!,
                SelectedExample.Value)
            .ToObservable()
            .FirstAsync()
            .ObserveOn(CurrentThreadScheduler.Instance);

        creationObs.Subscribe(
            maybeAnkiConnectEx => maybeAnkiConnectEx.Match(
                ankiConnectEx => _snackbarMessageQueue.Enqueue($"Card creating failed. {ankiConnectEx.Message}"),
                () => _snackbarMessageQueue.Enqueue("Card was successfully created")),
            executeEx => _snackbarMessageQueue.Enqueue($"Fail to execute. {executeEx.Message}"));

        creationObs
            .Catch(Observable.Empty<Option<Exception>>())
            .Subscribe(_ => IsCardCreating.TurnOff());
    }
}