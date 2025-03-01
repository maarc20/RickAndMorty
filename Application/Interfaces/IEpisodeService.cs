using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Application.Interfaces
{
    public interface IEpisodeService
    {
        Task<List<Episode>> GetAllEpisodesAsync();
        Task<Episode> GetEpisodeAsync(string EpisodeUrl);
        Task<List<Episode>> GetEpisodesAsync(List<int> EpisodeIds);
        Task<Dictionary<int, List<Episode>>> GetCharactersEpisodesAsync(List<CharacterAPI> characterAPIs);
        void InsertEpisodes(List<Episode> episodes);
    }
}