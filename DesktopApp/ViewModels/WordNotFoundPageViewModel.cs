using System.Collections.Generic;
using System.Linq;
using DesktopApp.Models;
using LongmanDictionary.Models.Pages;
using Reactive.Bindings;

namespace DesktopApp.ViewModels;

public class WordNotFoundPageViewModel : PageViewModel
{
    public WordNotFoundPageViewModel(WordNotFoundPage notFoundPage, LdSearcher searcher)
    {
        Suggestions = notFoundPage.SuggestionWords.Select(w => new SuggestionWordViewModel(w, searcher));
    }

    public IEnumerable<SuggestionWordViewModel> Suggestions { get; }

    public class SuggestionWordViewModel
    {
        public SuggestionWordViewModel(string word, LdSearcher searcher)
        {
            Word = word;

            SearchCommand = new ReactiveCommand();
            SearchCommand.Subscribe(() => searcher.Search(word));
        }

        public ReactiveCommand SearchCommand { get; }

        public string Word { get; }
    }
}