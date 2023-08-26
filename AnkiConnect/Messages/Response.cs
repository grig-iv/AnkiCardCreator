namespace AnkiConnect.Messages;

public class Response<TResult>
{
    public TResult? Result { get; init; }
    public string Error { get; init; }
    
    public bool HasError => !string.IsNullOrWhiteSpace(Error);
}