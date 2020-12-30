using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi_1.DTO.Character;
using WebApi_1.Models;

namespace WebApi_1.Services
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        private List<Character> _characters = new List<Character>{
          new Character(),
          new Character {Id = 1, Name = "Galdalf", Class = RpgClass.Mage}
        };


        public async Task<ServiceResponse<List<GetCharacterDTO>>> CreateCharacter(CreateCharacterDTO newCharacter)
        {



            _characters.Add(_mapper.Map<Character>(newCharacter));
            _characters.Last().Id = _characters.Max(c => c.Id) + 1; //since we are using list as our data store; we will have to manually update the id

            return new ServiceResponse<List<GetCharacterDTO>> { Data = (_characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList() };


        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                _characters.Remove(_characters.First(c => c.Id == id));
                serviceResponse.Data = (_characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();

            }
            catch (Exception ex)
            {

                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = ex.ToString();
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDTO>> { Data = (_characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList() };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(_characters.First(c => c.Id == id));
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.ToString();
                serviceResponse.Success = false;
            }

            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
        {

            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                var character = _characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
                character.Class = updatedCharacter.Class;
                character.Defense = updatedCharacter.Defense;
                character.HP = updatedCharacter.HP;
                character.MP = updatedCharacter.MP;
                character.Name = updatedCharacter.Name;
                character.Strength = updatedCharacter.Strength;

                serviceResponse.Data = _characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            }

            catch (Exception ex)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = ex.ToString();

            }

            return serviceResponse;
        }
    }
}