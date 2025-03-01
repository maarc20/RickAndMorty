using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Application.Interfaces
{
    public interface IEpisodeService
    {
        Task<List<Episode>> GetEpisodesAsync();

    }
}