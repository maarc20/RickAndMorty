using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Infrastructure.Interfaces
{
    public interface ICharacterRepository
    {
        List<Character> GetAllCharacters();
        void AddCharacter(CharacterAPI character);
        void AddCharacters(List<CharacterAPI> characters);
        List<Character>GetCharactersFiltered(Status status);
        Boolean DeleteCharacter(int CharacterId);
    }
}