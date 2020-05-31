
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace  dotnet_rpg.Services.CharacterService
{

    public class CharacterService : ICharacterService
    {
        public CharacterService(IMapper mapper,DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
private static List<Character> characters = new List<Character> {
    new Character(),
    new Character { Id = 1, Name = "Sam"}
};
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse=new ServiceResponse<List<GetCharacterDto>>();

             Character character=_mapper.Map<Character>(newCharacter);
             await _context.Characters.AddAsync(character);
             await _context.SaveChangesAsync();
            
            serviceResponse.Data= _context.Characters.Select(a=>_mapper.Map<GetCharacterDto>(a)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse=new ServiceResponse<List<GetCharacterDto>>();
            List<Character> dbCharacters = await _context.Characters.Where(c => c.User.Id == userId).ToListAsync();
            serviceResponse.Data=dbCharacters.Select(a=>_mapper.Map<GetCharacterDto>(a)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceResponse=new ServiceResponse<GetCharacterDto>();
           Character dbCharacter=await _context.Characters.FirstOrDefaultAsync(a=>a.Id==id);
            serviceResponse.Data=_mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse=new ServiceResponse<GetCharacterDto>();
            try
            {  
            Character character=await _context.Characters.FirstOrDefaultAsync(a=>a.Id==updateCharacter.Id);
            character.Defense=updateCharacter.Defense;
            character.HitPoints=updateCharacter.HitPoints;
            character.Intelligence=updateCharacter.Intelligence;
            character.Name=updateCharacter.Name;
            character.Class=updateCharacter.Class;
            character.Strength=updateCharacter.Strength;
            _context.Characters.Update(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data=_mapper.Map<GetCharacterDto>(character);
             }
            catch (System.Exception ex)
            { 
                 serviceResponse.Success=false;
                 serviceResponse.Message=ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
             ServiceResponse<List<GetCharacterDto>> serviceResponse=new ServiceResponse<List<GetCharacterDto>>();
             try
             {
                 Character character=await _context.Characters.FirstAsync(a=>a.Id==id);
                 _context.Characters.Remove(character);
                 await _context.SaveChangesAsync();
                 serviceResponse.Data=_context.Characters.Select(a=>_mapper.Map<GetCharacterDto>(a)).ToList();

             }
             catch (System.Exception ex)
             {
                serviceResponse.Success=false;
                serviceResponse.Message=ex.Message;
             }
             return serviceResponse;
        }
    }
}