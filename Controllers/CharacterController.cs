using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.CharacterService;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceRespone<List<GetCharacterResponseDto>>>> Get()
        {   
            return Ok( await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRespone<GetCharacterResponseDto>>> GetSingle(int id)
        {    
            return Ok( await _characterService.GetCharacterById(id));
        }
        [HttpPost]
        public async Task<ActionResult<ServiceRespone<List<GetCharacterResponseDto>>>> AddCharacter(AddCharacterRequestDto newCharacter)
        {    
            return Ok( await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceRespone<List<GetCharacterResponseDto>>>> UpdateCharacter(UpdateCharacterRequestDto updateCharacter)
        {    var respone = await _characterService.UpdateCharacter(updateCharacter);
            if(respone.Data is null)
            {
                return NotFound(respone);
            }
            return Ok( await _characterService.UpdateCharacter(updateCharacter));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceRespone<List<GetCharacterResponseDto>>>> DeleteCharacter(int id)
        {    var respone = await _characterService.DeleteCharacter (id);
            if(respone.Data is null)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }




        
    }
}