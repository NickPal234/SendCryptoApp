using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using SendCrypto.Application;
using SendCrypto.Application.Integration;
using SendCrypto.Application.Integration.Models;
using System.Net;
using System.Text.Json;

namespace SendCrypto.Test.Application;

public class RandomGeneratorApiWebClientTests
{
    private readonly RandomGeneratorWebClient _randomGeneratorApiWebClient;
    private readonly Mock<ICustomWebClient> _mockHttpClient;

    public RandomGeneratorApiWebClientTests()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockHttpClient = fixture.Freeze<Mock<ICustomWebClient>>();
        _randomGeneratorApiWebClient = fixture.Freeze<RandomGeneratorWebClient>();
    }

    [Fact]
    public async Task GetRandomAsync_ok_when_server_returns_ok()
    {
        var randomGeneratedValue = 1;
        var content = new RandomResponse(randomGeneratedValue);
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(content))
        };

        _mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

        var result = await _randomGeneratorApiWebClient.GetRandomWithRetryAsync();

        result.Should().Be(randomGeneratedValue);
    }


    [Fact]
    public async Task GetRandomAsync_failed_when_server_returns_internal_error()
    {
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.InternalServerError,
        };

        _mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

        var act = () => _randomGeneratorApiWebClient.GetRandomWithRetryAsync();

        await act.Should().ThrowAsync<Exception>();
        _mockHttpClient.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Exactly(Constants.MaxRetryCount));
    }

    [Fact]
    public async Task GetRandomAsync_failed_when_server_returns_too_bussy_many_times()
    {
        var response = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.TooManyRequests,
        };

        _mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

        var act = () => _randomGeneratorApiWebClient.GetRandomWithRetryAsync();

        await act.Should().ThrowAsync<Exception>();
        _mockHttpClient.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Exactly(Constants.MaxRetryCount));
    }
}