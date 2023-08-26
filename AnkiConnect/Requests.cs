using AnkiConnect.Messages;
using AnkiConnect.Models;
using CSharpFunctionalExtensions;

namespace AnkiConnect;

public static class Requests
{
    public static async Task<Result<long>> AddNoteAsync(this AnkiConnectClient client, Note note)
    {
        return await client.SendRequestAsync<long>(
            new Request("addNote") { Params = new { Note = note } }
        );
    }
}