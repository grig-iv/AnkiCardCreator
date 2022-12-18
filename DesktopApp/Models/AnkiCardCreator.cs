using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using AnkiConnect;
using AnkiConnect.Models;
using LongmanDictionary.Models;
using Optional;
using Optional.Collections;
using Reactive.Bindings;

namespace DesktopApp.Models;

public class AnkiCardCreator
{
    private readonly AnkiConnectClient _ankiConnect;

    public AnkiCardCreator()
    {
        _ankiConnect = new AnkiConnectClient();
    }

    public async Task<Option<Exception>> CreateAsync(string word, Entry entry, Sense sense, Example? example = null)
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
            Audio = new[] { maybeWordAudio, maybeExampleAudio }.Values().ToArray(),
            Options = new NoteOptions { AllowDuplicate = true },
        };

        var result = await _ankiConnect.AddNote(note);
        return result.Match(_ => Option.None<Exception>(), Option.Some);
    }

    private Option<Audio> CreateAudio(string? audioSource, string fieldName)
    {
        if (audioSource is null)
            return Option.None<Audio>();

        var srcEndPoint = audioSource.Split("/").Last();
        var filename = srcEndPoint.Remove(srcEndPoint.IndexOf("?"));
        return new Audio(audioSource, filename, new[] { fieldName }).Some();
    }
}