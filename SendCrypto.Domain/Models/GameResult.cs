namespace SendCrypto.Domain.Models;

public record GameResult(
    string Result,
    string PlayerChoice,
    int PlayerChoiceId,
    string BotChoice,
    int BotChoiceId);