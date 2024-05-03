using SendCrypto.Application.Integration;
using SendCrypto.Application.Services.Interfaces;
using SendCrypto.Domain.Models;

namespace SendCrypto.Application.Services.Implementation;

public class RpslsService : IRpslsService
{
    private readonly IRandomGeneratorWebClient _client;

    public const string RandomValueNotInRange = "Random value is not in range";

    public RpslsService(IRandomGeneratorWebClient client)
    {
        _client = client;
    }


    public List<Choice> GetChoices()
    {
        var result = new List<Choice>();
        var allSignals = GetAllSignals();

        foreach (var signal in allSignals)
        {
            result.Add(new Choice(signal.Key, signal.Value));
        }

        return result;
    }

    public async Task<Choice> GetRandomChoice()
    {
        var randomChoice = await GetRandomSignalAsync();
        return new Choice((int)randomChoice, randomChoice.ToString());
    }

    public async Task<GameResult> PlayAsync(int choiceId)
    {
        VerifyChoiceId(choiceId);

        var choicePlayer = (SignalType)choiceId;
        var choiceBot = await GetRandomSignalAsync();
        var result = GetResult(choicePlayer, choiceBot);

        return new GameResult(
            result,
            choicePlayer.ToString(),
            (int)choicePlayer,
            choiceBot.ToString(),
            (int)choiceBot);
    }

    private void VerifyChoiceId(int choiceId)
    {
        if (choiceId < 0 || choiceId > GetAllSignals().Count - 1)
        {
            throw new Exception("Is not correct value");
        }
    }

    private async Task<SignalType> GetRandomSignalAsync()
    {
        var random = await _client.GetRandomWithRetryAsync();

        if (random > Constants.MaxRandomNumber || random < Constants.MinRandomNumber)
        {
            throw new Exception(RandomValueNotInRange);
        }

        var enumNumber = random / Constants.RangeOfSignalTypes;

        return (SignalType)enumNumber;
    }

    private string GetResult(SignalType player, SignalType bot)
    {
        if (player == bot)
        {
            return ExodusType.Draw;
        }

        if (IsFirstPlayerWon(player, bot))
        {
            return ExodusType.Won;
        }

        return ExodusType.Lost;
    }

    private bool IsFirstPlayerWon(SignalType player, SignalType bot)
    {
        switch (player)
        {
            case SignalType.Rock: return Rock(bot);
            case SignalType.Paper: return Paper(bot);
            case SignalType.Scissors: return Scissors(bot);
            case SignalType.Lizard: return Lizard(bot);
            case SignalType.Spock: return Spock(bot);
            default: throw new Exception();
        }
    }

    private bool Spock(SignalType player2)
    {
        return player2 == SignalType.Scissors || player2 == SignalType.Rock;
    }

    private bool Lizard(SignalType player2)
    {
        return player2 == SignalType.Spock || player2 == SignalType.Paper;
    }

    private bool Scissors(SignalType player2)
    {
        return player2 == SignalType.Paper || player2 == SignalType.Lizard;
    }

    private bool Paper(SignalType player2)
    {
        return player2 == SignalType.Rock || player2 == SignalType.Spock;
    }

    private bool Rock(SignalType player2)
    {
        return player2 == SignalType.Scissors || player2 == SignalType.Lizard;
    }

    public Dictionary<int, string> GetAllSignals()
    {
        var list = new Dictionary<int, string>();
        foreach (SignalType val in Enum.GetValues(typeof(SignalType)))
        {
            list.Add((int)val, val.ToString());
        }

        return list;
    }
}