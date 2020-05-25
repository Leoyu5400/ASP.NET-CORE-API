using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_rpg.Controllers
{
[ApiController]
[Route("[Controller]")]
public class CharacterController:ControllerBase
{
 private static Character Knight=new Character();

[HttpGet]
 public IActionResult Get()
 {
     return Ok(Knight);
 }
}

}
