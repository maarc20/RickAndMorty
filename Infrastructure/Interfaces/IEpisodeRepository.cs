using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Infrastructure.Interfaces
{
    public interface IEpisodeRepository
    {
        List<Episode> GetAllEpisodes();
        void AddEpisode(Episode episode);
        void AddEpisodes(IEnumerable<Episode> episodes);
    }
}