namespace AnkiConnect.Models;

public record NoteOptions()
{
    public bool AllowDuplicate { get; init; }
    public string? DuplicateScope { get; init; }
    public DuplicateScopeOptions? DuplicateScopeOptions { get; init; }
}