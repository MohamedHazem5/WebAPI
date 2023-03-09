global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace WebAPI.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataContext _context { get; }
        public CharacterService(IMapper mapper,DataContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper =mapper;
        }


        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);
        public async Task<ServiceRespone<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter)
        {
            var ServiceRespone = new ServiceRespone<List<GetCharacterResponseDto>>();
            var character =_mapper.Map<Character>(newCharacter);

            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.characters.Add(character);
            await _context.SaveChangesAsync();
            ServiceRespone.Data= await _context.characters
            .Where(c => c.User!.Id == GetUserId())
            .Select(c=>_mapper.Map<GetCharacterResponseDto>(c))
            .ToListAsync();
            return ServiceRespone;
        }
        
        public async Task<ServiceRespone<List<GetCharacterResponseDto>>> DeleteCharacter(int id)
        {
            var ServiceRespone = new ServiceRespone<List<GetCharacterResponseDto>>();
            try{
            var character = await _context.characters
                .FirstOrDefaultAsync(c=>c.Id==id && c.User!.Id == GetUserId());
            if(character is null)
                throw new Exception($"Character with Id '{id}' is not found");
            _context.characters.Remove(character);
            await _context.SaveChangesAsync();
            ServiceRespone.Data=
                                await _context.characters
                                .Where(c=> c.User!.Id == GetUserId())
                                .Select(c=> _mapper.Map<GetCharacterResponseDto>(c)).ToListAsync();    
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
            var dbcharacters = await _context.characters.Where(c => c.User!.Id == GetUserId()).ToListAsync();
            ServiceRespone.Data=dbcharacters.Select(c=>_mapper.Map<GetCharacterResponseDto>(c)).ToList();
            return ServiceRespone;
        }

        public async Task<ServiceRespone<GetCharacterResponseDto>> GetCharacterById(int id)
        {
            var ServiceRespone = new ServiceRespone<GetCharacterResponseDto>();
            var dbCharacter = await _context.characters
            .FirstOrDefaultAsync(c=>c.Id==id && c.User!.Id == GetUserId());
            ServiceRespone.Data=_mapper.Map<GetCharacterResponseDto>(dbCharacter);
            return ServiceRespone;

            
        }

        public async Task<ServiceRespone<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterRequestDto updateCharacter)
        {
            var ServiceRespone = new ServiceRespone<GetCharacterResponseDto>();
            try{
            var character = await _context.characters
            .Include(c=>c.User)
            .FirstOrDefaultAsync(c=>c.Id==updateCharacter.Id);
            if(character is null || character.User!.Id == GetUserId()) 
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