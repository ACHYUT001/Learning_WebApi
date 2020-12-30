using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi_1.Data;
using WebApi_1.DTO.Character;
using WebApi_1.Models;

namespace WebApi_1.Services
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        private List<Character> _characters = new List<Character>{
          new Character(),
          new Character {Id = 1, Name = "Galdalf", Class = RpgClass.Mage}
        };


        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            List<Character> dbCharacters = await _context.Characters.ToListAsync();
            return new ServiceResponse<List<GetCharacterDTO>> { Data = (dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList() };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                Character dbCharacter = await _context.Characters.FirstAsync(c => c.Id == id);
                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.ToString();
                serviceResponse.Success = false;
            }

            return serviceResponse;

        }
        public async Task<ServiceResponse<List<GetCharacterDTO>>> CreateCharacter(CreateCharacterDTO newCharacter)
        {

            await _context.Characters.AddAsync(_mapper.Map<Character>(newCharacter));
            await _context.SaveChangesAsync();

            // _characters.Add(_mapper.Map<Character>(newCharacter));
            // _characters.Last().Id = _characters.Max(c => c.Id) + 1; //since we are using list as our data store; we will have to manually update the id

            return new ServiceResponse<List<GetCharacterDTO>> { Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList() };


        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                _context.Characters.Remove(await _context.Characters.FirstAsync(c => c.Id == id));
                await _context.SaveChangesAsync();

                serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c))).ToList();

            }
            catch (Exception ex)
            {

                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = ex.ToString();
            }

            return serviceResponse;
        }



        public async Task<ServiceResponse<List<GetCharacterDTO>>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
        {

            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                character.Class = updatedCharacter.Class;
                character.Defense = updatedCharacter.Defense;
                character.HP = updatedCharacter.HP;
                character.MP = updatedCharacter.MP;
                character.Name = updatedCharacter.Name;
                character.Strength = updatedCharacter.Strength;

                _context.Characters.Update(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
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