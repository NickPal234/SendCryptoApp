namespace SendCrypto.Application.Integration;

public interface IRandomGeneratorWebClient
{
    Task<int> GetRandomWithRetryAsync();
    Task<int> GetRandomAsync();
}