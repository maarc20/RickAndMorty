using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Infrastructure.Interfaces
{
    public interface ICharacterRepository
    {
        List<Character> GetAllCharacters();
        void AddCharacter(Character character);
        void AddCharacters(List<Character> characters);
    }
}