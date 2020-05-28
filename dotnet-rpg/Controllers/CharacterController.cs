using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_rpg.Controllers
{
[ApiController]
[Route("[Controller]")]
public class CharacterController:ControllerBase
{
    private readonly ICharacterService _characterService;
    public CharacterController(ICharacterService characterService)
    {
            _characterService = characterService;
        }

[HttpGet("GetAll")]
 public async Task<IActionResult> Get()
 {
     return Ok( await _characterService.GetAllCharacters());
 }

[HttpGet("{id}")]
 public async Task<IActionResult> GetSingle(int id)
 {
     return Ok(await _characterService.GetCharacterById(id));
 }

[HttpPost]
 public async Task<IActionResult> AddCharacter(AddCharacterDto newCharacter)
 {
   return Ok(await _characterService.AddCharacter(newCharacter));
 }

 [HttpPut]
 public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updateCharacter)
 {
    ServiceResponse<GetCharacterDto> serviceResponse= await _characterService.UpdateCharacter(updateCharacter);
    if(serviceResponse.Data ==null)
    {
        return NotFound(serviceResponse);
    }
     return Ok(serviceResponse);
 }

 [HttpDelete]
 public async Task<IActionResult> DeleteCharacter(int id)
 {
     ServiceResponse<List<GetCharacterDto>> serviceResponse= await _characterService.DeleteCharacter(id);
     if(serviceResponse==null)
     {
         return NotFound(serviceResponse);

     }
     return Ok(serviceResponse);
 }
}
 
}
