using AutoMapper;
using SendCrypto.Domain.Models;
using SendCrypto.WebApi.Models;

namespace SendCrypto.WebApi.Mapper;

public class ModelViewProfile : Profile
{
    public ModelViewProfile()
    {
        CreateMap<ChoiceViewModel, Choice>().ReverseMap();
        CreateMap<GameResultViewModel, GameResult>().ReverseMap();
    }
}