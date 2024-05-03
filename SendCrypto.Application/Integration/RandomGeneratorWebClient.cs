using SendCrypto.Application.Integration.Models;
using System.Net.Http.Json;

namespace SendCrypto.Application.Integration;

public class RandomGeneratorWebClient : IRandomGeneratorWebClient
{
    public string ResultContentError = "Response is not correct";
    public string StatusCodeIsNoValid = "Status code is not valid from random generator site";
    public string RetryPatternIsBroken = "RetryPattern - is broken";

    private readonly ICustomWebClient _client;
    public RandomGeneratorWebClient(ICustomWebClient client)
    {
        _client = client;
    }

    public async Task<int> GetRandomWithRetryAsync()
    {
        int retryCount = Constants.MaxRetryCount;
        while (retryCount > 0)
        {
            try
            {
                return await GetRandomAsync();
            }
            catch (Exception)
            {
                retryCount--;
                if (retryCount <= 0) throw;
            }
        }

        throw new Exception(RetryPatternIsBroken);
    }

    public async Task<int> GetRandomAsync()
    {
        var result = await _client.GetAsync("/random");

        if (result.IsSuccessStatusCode)
        {
            var resultContent = await result.Content.ReadFromJsonAsync<RandomResponse>();

            if (resultContent is null || !resultContent.Random.HasValue)
            {
                throw new Exception(ResultContentError);
            }

            return resultContent.Random.Value;
        }

        throw new Exception(StatusCodeIsNoValid);
    }
}