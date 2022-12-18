namespace AnkiConnect.Models;

public record Audio(string Url, string Filename, ICollection<string> Fields)
{
    public string? SkipHash { get; init; }
}