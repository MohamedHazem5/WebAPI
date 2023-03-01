global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebAPI.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        public DataContext _context { get; }
        public CharacterService(IMapper mapper,DataContext context)
        {
            _context = context;
             _mapper=mapper;
        }
        public async Task<ServiceRespone<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter)
        {
            var ServiceRespone = new ServiceRespone<List<GetCharacterResponseDto>>();
            var character =_mapper.Map<Character>(newCharacter);
            _context.characters.Add(character);
            await _context.SaveChangesAsync();
            ServiceRespone.Data=await _context.characters.Select(c=>_mapper.Map<GetCharacterResponseDto>(c)).ToListAsync();
            return ServiceRespone;
        }
        
        public async Task<ServiceRespone<List<GetCharacterResponseDto>>> DeleteCharacter(int id)
        {
            var ServiceRespone = new ServiceRespone<List<GetCharacterResponseDto>>();
            try{
            var character = await _context.characters.FirstOrDefaultAsync(c=>c.Id==id);
            if(character is null)
                throw new Exception($"Character with Id '{id}' is not found");
            _context.characters.Remove(character);
            await _context.SaveChangesAsync();
            ServiceRespone.Data=
                                await _context.characters.Select(c=> _mapper.Map<GetCharacterResponseDto>(c)).ToListAsync();    
            }
            catch (Exception ex)
            {
                ServiceRespone.Sucess=false;
                ServiceRespone.Message=ex.Message;
            }
            return ServiceRespone;
        }

        public async Task<ServiceRespone<List<GetCharacterResponseDto>>> GetAllCharacters()
        {
            var ServiceRespone = new ServiceRespone<List<GetCharacterResponseDto>>();
            var dbcharacters = await _context.characters.ToListAsync();
            ServiceRespone.Data=dbcharacters.Select(c=>_mapper.Map<GetCharacterResponseDto>(c)).ToList();
            return ServiceRespone;
        }

        public async Task<ServiceRespone<GetCharacterResponseDto>> GetCharacterById(int id)
        {
            var ServiceRespone = new ServiceRespone<GetCharacterResponseDto>();
            var dbCharacter = await _context.characters.FirstOrDefaultAsync(c=>c.Id==id);
            ServiceRespone.Data=_mapper.Map<GetCharacterResponseDto>(dbCharacter);
            return ServiceRespone;

            
        }

        public async Task<ServiceRespone<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterRequestDto updateCharacter)
        {
            var ServiceRespone = new ServiceRespone<GetCharacterResponseDto>();
            try{
            var character = await _context.characters.FirstOrDefaultAsync(c=>c.Id==updateCharacter.Id);
            if(character is null)
                throw new Exception($"Character with Id '{updateCharacter.Id}' is not found");
            character.Name= updateCharacter.Name;
            character.HitPoints= updateCharacter.HitPoints;
            character.Strength= updateCharacter.Strength;
            character.Defense= updateCharacter.Defense;
            character.Intelligence= updateCharacter.Intelligence;
            character.Class= updateCharacter.Class;

            await _context.SaveChangesAsync();
            ServiceRespone.Data=_mapper.Map<GetCharacterResponseDto>(character);    
            }
            catch (Exception ex)
            {
                ServiceRespone.Sucess=false;
                ServiceRespone.Message=ex.Message;
            }
            return ServiceRespone;
        }
    }
}