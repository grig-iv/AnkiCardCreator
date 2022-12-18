using System.Diagnostics;
using System.Net;
using AnkiConnect.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Optional;

namespace AnkiConnect;

public class AnkiConnectClient
{
    private const string AnkiConnectUri = "http://localhost:8765";
    private readonly HttpClient _client;
    private readonly JsonSerializerSettings _jsonSettings;

    public AnkiConnectClient()
    {
        _client = new HttpClient();
        _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Debugger.IsAttached ? Formatting.Indented : Formatting.None,
        };
    }

    public async Task<Option<TResponse, Exception>> SendRequestAsync<TResponse>(Request request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var jsonRequest = JsonConvert.SerializeObject(request, _jsonSettings);
        var httpResponse = await _client.PostAsync(AnkiConnectUri, new StringContent(jsonRequest));
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Response<TResponse>>(jsonResponse, _jsonSettings);

        return string.IsNullOrWhiteSpace(response.Error)
            ? Option.Some<TResponse, Exception>(response.Result!)
            : Option.None<TResponse, Exception>(new Exception(response.Error));
    }
}