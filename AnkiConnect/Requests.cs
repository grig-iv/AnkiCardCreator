using AnkiConnect.Messages;
using AnkiConnect.Models;
using Optional;

namespace AnkiConnect;

public static class Requests
{
    public static Task<Option<long, Exception>> AddNote(this AnkiConnectClient client, Note note)
    {
        return client.SendRequestAsync<long>(
            new Request("addNote") { Params = new { Note = note } }
        );
    }
}