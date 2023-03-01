namespace WebAPI.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceRespone<List<GetCharacterResponseDto>>> GetAllCharacters();
        Task<ServiceRespone<GetCharacterResponseDto>> GetCharacterById(int id);
        Task<ServiceRespone<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter);
         Task<ServiceRespone<GetCharacterResponseDto>> UpdateCharacter(UpdateCharacterRequestDto updateCharacter);
        Task<ServiceRespone<List<GetCharacterResponseDto>>> DeleteCharacter(int id);
    }
}