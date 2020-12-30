using AutoMapper;
using WebApi_1.DTO.Character;
using WebApi_1.Models;

namespace WebApi_1
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<CreateCharacterDTO, Character>();
        }
    }
}