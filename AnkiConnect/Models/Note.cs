namespace AnkiConnect.Models;

public record Note(string DeckName, string ModelName, IReadOnlyDictionary<string, string> Fields)
{
    public NoteOptions? Options { get; init; }
    public ICollection<string>? Tags { get; init; }
    public Audio[]? Audio { get; init; }
}