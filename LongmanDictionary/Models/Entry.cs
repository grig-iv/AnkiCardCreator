using System.Diagnostics;
using CSharpFunctionalExtensions;
using HtmlAgilityPack;
using LongmanDictionary.Utils;

namespace LongmanDictionary.Models;

[DebuggerDisplay("{Pronunciation} {PartOfSpeech}")]
public record Entry
{
    public Entry(HtmlNode entryNode)
    {
        Hyphenation = entryNode
            .SelectSingleNode(".//span[@class='HYPHENATION']")
            .InnerPrettyText();
        
        Pronunciation = entryNode
            .SelectSingleNode(".//span[@class='PRON']")
            .InnerPrettyText();

        FrequencyLevel = entryNode
            .SelectSingleNode(".//span[@class='tooltip LEVEL']")
            .InnerPrettyText()
            .AsMaybe()
            .Select(text => text switch
            {
                "●○○" => FrequencyLevel.Low,
                "●●○" => FrequencyLevel.Medium,
                "●●●" => FrequencyLevel.High,
                _ => FrequencyLevel.None,
            })
            .GetValueOrDefault();
            
        PartOfSpeech = entryNode
            .SelectSingleNode(".//span[@class='POS']")
            .InnerPrettyText();
        
        AmericanWordAudioSrc = entryNode
            .SelectSingleNode(".//span[@class='speaker amefile fas fa-volume-up hideOnAmp']")
            ?.GetAttributeValue("data-src-mp3", null);

        Senses = entryNode
            .SelectNodes(".//span[@class='Sense']")
            ?.Select(node => new Sense(node) )
            .ToList() ?? new List<Sense>();
    }
    
    public string? Hyphenation { get; }
    public string? Pronunciation { get; }
    public FrequencyLevel FrequencyLevel { get; } 
    public string? PartOfSpeech { get; }
    public string? AmericanWordAudioSrc { get; }
    public IReadOnlyList<Sense> Senses { get; }
}