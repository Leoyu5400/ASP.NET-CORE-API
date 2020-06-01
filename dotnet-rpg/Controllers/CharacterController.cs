using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_rpg.Controllers
{
 [Authorize]
[ApiController]
[Route("[Controller]")]
public class CharacterController:ControllerBase
{
    private readonly ICharacterService _characterService;
    public CharacterController(ICharacterService characterService)
    {
            _characterService = characterService;
        }

// [AllowAnonymous]
[HttpGet("GetAll")]
 public async Task<IActionResult> Get()
 {
  int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
    return Ok(await _characterService.GetAllCharacters());
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
