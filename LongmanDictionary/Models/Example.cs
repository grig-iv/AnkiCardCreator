using System.Diagnostics;
using CSharpFunctionalExtensions;
using HtmlAgilityPack;
using LongmanDictionary.Utils;

namespace LongmanDictionary.Models;

[DebuggerDisplay("{Sentence}")]
public record Example
{
    public Example(HtmlNode exampleNode)
    {
        ProperForm = exampleNode
            .SelectNodes("//span[@class='PROPFORM'] | //span[@class='PROPFORMPREP']")
            ?.TryFirst(node => node.ParentNode.ChildNodes.Contains(exampleNode))
            .Select(node => node.InnerPrettyText())
            .GetValueOrDefault();
        
        Sentence = exampleNode.InnerPrettyText()!;
        
        AudioSrc = exampleNode
            .SelectSingleNode(".//span[@data-src-mp3]")
            ?.GetAttributeValue("data-src-mp3", null);
    }
    
    public string? ProperForm { get; }
    public string Sentence { get; }
    public string? AudioSrc { get; }
}