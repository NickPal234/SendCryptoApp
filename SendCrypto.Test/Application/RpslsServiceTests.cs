using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using SendCrypto.Application;
using SendCrypto.Application.Integration;
using SendCrypto.Application.Services.Implementation;
using SendCrypto.Domain.Models;

namespace SendCrypto.Test.Application;

public class RpslsServiceTests
{
    private readonly RpslsService _rpslsService;
    private readonly Mock<IRandomGeneratorWebClient> _webClientMock;

    public RpslsServiceTests()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        _webClientMock = fixture.Freeze<Mock<IRandomGeneratorWebClient>>();
        _rpslsService = fixture.Freeze<RpslsService>();
    }

    [Theory]
    [InlineData(SignalType.Rock)]
    [InlineData(SignalType.Paper)]
    [InlineData(SignalType.Scissors)]
    [InlineData(SignalType.Lizard)]
    [InlineData(SignalType.Spock)]
    public async Task GetRandomChoice_ok_when_data_is_correct(SignalType expectedType)
    {
        var randomValue = (int)expectedType * Constants.RangeOfSignalTypes;

        _webClientMock.Setup(x => x.GetRandomWithRetryAsync()).ReturnsAsync(randomValue);

        var result = await _rpslsService.GetRandomChoice();

        result.Id.Should().Be((int)expectedType);
        result.Name.Should().Be(expectedType.ToString());
    }

    [Theory]
    [InlineData(Constants.MaxRandomNumber + 1)]
    [InlineData(Constants.MaxRandomNumber + 100)]
    [InlineData(Constants.MinRandomNumber - 1)]
    [InlineData(Constants.MinRandomNumber - 100)]
    public async Task GetRandomChoice_failed_when_random_value_is_out_of_range(int randomValue)
    {
        _webClientMock.Setup(x => x.GetRandomWithRetryAsync()).ReturnsAsync(randomValue);

        var act = () => _rpslsService.GetRandomChoice();

        await act.Should().ThrowAsync<Exception>().WithMessage(RpslsService.RandomValueNotInRange);
    }

    [Theory]
    [InlineData(Constants.MaxRandomNumber)]
    [InlineData(Constants.MaxRandomNumber - 1)]
    [InlineData(Constants.MinRandomNumber)]
    [InlineData(Constants.MinRandomNumber + 1)]
    public async Task GetRandomChoice_ok_when_random_value_is_in_range(int randomValue)
    {
        _webClientMock.Setup(x => x.GetRandomWithRetryAsync()).ReturnsAsync(randomValue);

        var act = () => _rpslsService.GetRandomChoice();

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData(SignalType.Rock, SignalType.Rock, ExodusType.Draw)]
    [InlineData(SignalType.Rock, SignalType.Scissors, ExodusType.Won)]
    [InlineData(SignalType.Rock, SignalType.Lizard, ExodusType.Won)]
    [InlineData(SignalType.Rock, SignalType.Paper, ExodusType.Lost)]
    [InlineData(SignalType.Rock, SignalType.Spock, ExodusType.Lost)]
    public async Task PlayAsync_for_rock(SignalType player, SignalType bot, string expectedResult)
    {
        var randomValue = (int)bot * Constants.RangeOfSignalTypes;
        _webClientMock.Setup(x => x.GetRandomWithRetryAsync()).ReturnsAsync(randomValue);

        var result = await _rpslsService.PlayAsync((int)player);

        result.Result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(SignalType.Scissors, SignalType.Scissors, ExodusType.Draw)]
    [InlineData(SignalType.Scissors, SignalType.Paper, ExodusType.Won)]
    [InlineData(SignalType.Scissors, SignalType.Lizard, ExodusType.Won)]
    [InlineData(SignalType.Scissors, SignalType.Rock, ExodusType.Lost)]
    [InlineData(SignalType.Scissors, SignalType.Spock, ExodusType.Lost)]
    public async Task PlayAsync_for_scissors(SignalType player, SignalType bot, string expectedResult)
    {
        var randomValue = (int)bot * Constants.RangeOfSignalTypes;
        _webClientMock.Setup(x => x.GetRandomWithRetryAsync()).ReturnsAsync(randomValue);

        var result = await _rpslsService.PlayAsync((int)player);

        result.Result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(SignalType.Paper, SignalType.Paper, ExodusType.Draw)]
    [InlineData(SignalType.Paper, SignalType.Rock, ExodusType.Won)]
    [InlineData(SignalType.Paper, SignalType.Spock, ExodusType.Won)]
    [InlineData(SignalType.Paper, SignalType.Lizard, ExodusType.Lost)]
    [InlineData(SignalType.Paper, SignalType.Scissors, ExodusType.Lost)]
    public async Task PlayAsync_for_paper(SignalType player, SignalType bot, string expectedResult)
    {
        var randomValue = (int)bot * Constants.RangeOfSignalTypes;
        _webClientMock.Setup(x => x.GetRandomWithRetryAsync()).ReturnsAsync(randomValue);

        var result = await _rpslsService.PlayAsync((int)player);

        result.Result.Should().Be(expectedResult);
    }
}