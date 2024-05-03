namespace SendCrypto.WebApi.Models;

public record GameResultViewModel(
    string Result,
    string PlayerChoice,
    int PlayerChoiceId,
    string BotChoice,
    int BotChoiceId);