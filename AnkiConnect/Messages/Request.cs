namespace AnkiConnect.Messages;

public record Request(string Action)
{
    public int Version => 6;
    public object? Params { get; init; }
}