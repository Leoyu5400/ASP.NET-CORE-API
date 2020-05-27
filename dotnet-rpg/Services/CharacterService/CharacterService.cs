
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace  dotnet_rpg.Services.CharacterService
{

    public class CharacterService : ICharacterService
    {
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }
private static List<Character> characters = new List<Character> {
    new Character(),
    new Character { Id = 1, Name = "Sam"}
};
        private readonly IMapper _mapper;

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse=new ServiceResponse<List<GetCharacterDto>>();
             Character character=_mapper.Map<Character>(newCharacter);
              character.Id=characters.Max(a=>a.Id)+1;
            characters.Add(character);
            
            serviceResponse.Data=characters.Select(a=>_mapper.Map<GetCharacterDto>(a)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse=new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data=characters.Select(a=>_mapper.Map<GetCharacterDto>(a)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceResponse=new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data=_mapper.Map<GetCharacterDto>(characters.FirstOrDefault(a=>a.Id==id));
            return serviceResponse;
        }
    }
}