using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendCrypto.Application.Services.Interfaces;
using SendCrypto.WebApi.Models;

namespace SendCrypto.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RpslsGameController : ControllerBase
{
    private readonly IRpslsService _rpslsService;
    private readonly IMapper _mapper;

    public RpslsGameController(
        IRpslsService rpslsService,
        IMapper mapper)
    {
        _rpslsService = rpslsService;
        _mapper = mapper;
    }


    [HttpGet("choices")]
    [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 300)]
    public ActionResult Choices()
    {
        var choices = _rpslsService.GetChoices();
        var result = _mapper.Map<List<ChoiceViewModel>>(choices);
        return Ok(result);
    }

    [HttpGet("choice")]
    public async Task<ActionResult> Choice()
    {
        var choice = await _rpslsService.GetRandomChoice();
        var result = _mapper.Map<ChoiceViewModel>(choice);
        return Ok(result);
    }

    [HttpPost("play")]
    public async Task<ActionResult> Play(int choseId)
    {
        var gameResult = await _rpslsService.PlayAsync(choseId);
        var result = _mapper.Map<GameResultViewModel>(gameResult);
        return Ok(result);
    }
}
