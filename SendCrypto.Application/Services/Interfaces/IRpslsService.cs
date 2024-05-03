using SendCrypto.Domain.Models;

namespace SendCrypto.Application.Services.Interfaces;

public interface IRpslsService
{
    public Task<Choice> GetRandomChoice();
    public Task<GameResult> PlayAsync(int choiceId);
    public List<Choice> GetChoices();
}