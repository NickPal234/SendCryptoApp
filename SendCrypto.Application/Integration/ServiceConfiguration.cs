using Microsoft.Extensions.DependencyInjection;

namespace SendCrypto.Application.Integration;

public static class ServiceConfiguration
{
    public static void ConfigureRandomGeneratorWebClient(this IServiceCollection services)
    {
        services.AddScoped<IRandomGeneratorWebClient, RandomGeneratorWebClient>();

        services.AddHttpClient<ICustomWebClient, CustomWebClient>(
            (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(Constants.RandomGeneratorUrl);
                client.Timeout = TimeSpan.FromSeconds(5);
            });
    }
}