using System.Diagnostics;
using HtmlAgilityPack;
using LongmanDictionary.Utils;

namespace LongmanDictionary.Models;

[DebuggerDisplay("{Signpost} {Definition}")]
public record Sense
{
    public Sense(HtmlNode senseNode)
    {
        Definition = senseNode
            .SelectSingleNode(".//span[@class='DEF']")
            .InnerPrettyText();

        Signpost = senseNode
            .SelectSingleNode(".//span[@class='SIGNPOST']")
            .InnerPrettyText();

        Synonym = senseNode
            .SelectSingleNode(".//span[@class='SYN']")
            .InnerPrettyText()
            ?.Remove(0, 3)  // Removing "SYN"
            .Trim();

        Opposition = senseNode
            .SelectSingleNode(".//span[@class='OPP']")
            .InnerPrettyText();

        RegisterLabel = senseNode
            .SelectSingleNode(".//span[@class='REGISTERLAB']")
            .InnerPrettyText();

        Examples = senseNode
            .SelectNodes(".//span[@class='EXAMPLE']")
            ?.Select(node => new Example(node))
            .ToList() ?? new List<Example>();
    }

    public string? Definition { get; }
    public string? Signpost { get; }
    public string? Synonym { get; }
    public string? Opposition { get; }
    public string? RegisterLabel { get; }
    public string? ProperForm { get; }
    public IReadOnlyList<Example> Examples { get; }
}