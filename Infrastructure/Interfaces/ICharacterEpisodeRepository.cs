using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Infrastructure.Interfaces
{
    public interface ICharacterEpisodeRepository
    {
        List<CharacterEpisode> GetAllCharacterEpisodes();
        void AddCharacterEpisodes(Dictionary<int, List<Episode>> CharacterEpisodes);
        void DeleteEpisodesFromCaracterId(int CharacterId);
        List<int> GetEpisodeIdsFromCharacterId(int Characterid);
    }
}