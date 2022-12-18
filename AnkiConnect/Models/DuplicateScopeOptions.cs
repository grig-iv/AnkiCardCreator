namespace AnkiConnect.Models;

public record DuplicateScopeOptions(
    string DeckName,
    bool CheckChildren,
    bool CheckAllModels
);