namespace SendCrypto.Application.Integration;

public class CustomWebClient : ICustomWebClient
{
    private readonly HttpClient _client;

    public CustomWebClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        return await _client.GetAsync(requestUri);
    }
}