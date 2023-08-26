using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiConnect;
using AnkiConnect.Models;
using CSharpFunctionalExtensions;
using LongmanDictionary.Models;

namespace DesktopApp.Models;

public class AnkiCardCreator
{
    private readonly AnkiConnectClient _ankiConnect;

    public AnkiCardCreator()
    {
        _ankiConnect = new AnkiConnectClient();
    }

    public async Task<Result> CreateAsync(string word, Entry entry, Sense sense, Example? example = null)
    {
        var fields = new Dictionary<string, string>
        {
            ["Word"] = word,
            ["Pronunciation"] = entry.Pronunciation ?? string.Empty,
            ["PartOfSpeech"] = entry.PartOfSpeech ?? string.Empty,
            ["Definition"] = sense.Definition ?? string.Empty,
            ["Signpost"] = sense.Signpost ?? string.Empty,
            ["Synonym"] = sense.Synonym ?? string.Empty,
            ["Opposition"] = sense.Opposition ?? string.Empty,
            ["Sentence"] = example?.Sentence ?? string.Empty,
            ["RegisterLabels"] = sense.RegisterLabel ?? string.Empty,
            ["ProperForm"] = example?.ProperForm ?? string.Empty,
            ["_cardVersion"] = "v1",
        };

        var maybeWordAudio = CreateAudio(entry.AmericanWordAudioSrc, "wordAudio");
        var maybeExampleAudio = CreateAudio(example?.AudioSrc, "sentenceAudio");
        var note = new Note("My English Words", "My English Card Template", fields)
        {
            Audio = new[] { maybeWordAudio, maybeExampleAudio }.Choose().ToArray(),
            Options = new NoteOptions { AllowDuplicate = true },
        };

        return await _ankiConnect.AddNoteAsync(note);
    }

    private Maybe<Audio> CreateAudio(string? audioSource, string fieldName)
    {
        // source example:
        // https://www.ldoceonline.com/media/english/breProns/popsicle0205.mp3?version=1.2.63
        return audioSource
            .AsMaybe()
            .Where(src => !string.IsNullOrWhiteSpace(src))
            .SelectMany(GetFileName)
            .SelectMany(RemoveVersion)
            .Select(filename => new Audio(audioSource!, filename, new[] {fieldName}));
    }

    private Maybe<string> GetFileName(string audioSource)
    {
       return audioSource.Split("/").TryLast();
    }

    private Maybe<string> RemoveVersion(string audioSource)
    {
        // ReSharper disable once StringLastIndexOfIsCultureSpecific.1
        var versionIndex = audioSource.LastIndexOf("?");
        return versionIndex != -1 
            ? audioSource.Remove(versionIndex) 
            : Maybe<string>.None;
    }
}