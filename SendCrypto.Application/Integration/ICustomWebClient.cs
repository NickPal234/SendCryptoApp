namespace SendCrypto.Application.Integration;

public interface ICustomWebClient
{
    public Task<HttpResponseMessage> GetAsync(string requestUri);
}