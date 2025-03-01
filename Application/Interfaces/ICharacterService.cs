using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Application.Interfaces
{
    public interface ICharacterService
    {
        Task<List<CharacterAPI>> GetAllCharactersAsync();
    }
}