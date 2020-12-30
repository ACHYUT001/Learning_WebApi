using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_1.DTO.Character;
using WebApi_1.Models;

namespace WebApi_1.Services
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters();
        Task<ServiceResponse<GetCharacterDTO>> GetCharacter(int id);

        Task<ServiceResponse<List<GetCharacterDTO>>> CreateCharacter(CreateCharacterDTO newCharacter);

        Task<ServiceResponse<List<GetCharacterDTO>>> UpdateCharacter(UpdateCharacterDTO updatedCharacter);

        Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id);


    }
}