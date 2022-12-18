using System.Diagnostics;
using HtmlAgilityPack;
using LongmanDictionary.Utils;
using Optional.Collections;
using Optional.Unsafe;

namespace LongmanDictionary.Models;

[DebuggerDisplay("{Sentence}")]
public record Example
{
    public Example(HtmlNode exampleNode)
    {
        ProperForm = exampleNode
            .SelectNodes("//span[@class='PROPFORM'] | //span[@class='PROPFORMPREP']")
            ?.FirstOrNone(node => node.ParentNode.ChildNodes.Contains(exampleNode))
            .Map(node => node.InnerPrettyText())
            .ValueOrDefault();
        
        Sentence = exampleNode.InnerPrettyText()!;
        
        AudioSrc = exampleNode
            .SelectSingleNode(".//span[@data-src-mp3]")
            ?.GetAttributeValue("data-src-mp3", null);
    }
    
    public string? ProperForm { get; }
    public string Sentence { get; }
    public string? AudioSrc { get; }
}