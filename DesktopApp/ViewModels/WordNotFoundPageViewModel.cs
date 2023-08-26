using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DesktopApp.Models;
using LongmanDictionary.Models.Pages;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DesktopApp.ViewModels;

public class WordNotFoundPageViewModel : PageViewModel<WordNotFoundPage>
{
    public WordNotFoundPageViewModel(LdSearcher searcher)
        : base(searcher)
    {
        WhenPageLoaded
            .Select(p => p.SuggestionWords.Select(w => new SuggestionWordViewModel(w, searcher)))
            .ToPropertyEx(this, x => x.Suggestions);
    }

    [ObservableAsProperty] public IEnumerable<SuggestionWordViewModel>? Suggestions { get; }

    public class SuggestionWordViewModel
    {
        public SuggestionWordViewModel(string word, LdSearcher searcher)
        {
            Word = word;

            SearchCommand = ReactiveCommand.Create(() => searcher.Search(word));
        }

        public ReactiveCommand<Unit, SearchRequest> SearchCommand { get; }

        public string Word { get; }
    }
}