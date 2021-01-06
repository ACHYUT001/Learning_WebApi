using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_1.DTO.Character;
using WebApi_1.Models;
using WebApi_1.Services;

namespace WebApi_1.Controllers
{
    //all requests needs to authenticated 
    [Authorize]
    //indicates that the types and all of it's dervived types are used to serve http requests
    //also enables features like http routing and responses
    [ApiController]
    //setup http routes to the controller
    [Route("[controller]")]
    //ControllerBase is used when we want mvc features w/o Views
    public class CharacterController : ControllerBase
    {

        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await _characterService.GetAllCharacters(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResponse<GetCharacterDTO> response = await _characterService.GetCharacter(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(CreateCharacterDTO character)
        {

            return Ok(await _characterService.CreateCharacter(character));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put(UpdateCharacterDTO updatedCharacter)
        {

            ServiceResponse<List<GetCharacterDTO>> response = await _characterService.UpdateCharacter(updatedCharacter);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            ServiceResponse<List<GetCharacterDTO>> response = await _characterService.DeleteCharacter(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);



        }



    }
}