using PruebaEurofirms.Domain.Entities;
using PruebaEurofirms.Application.Interfaces;
using PruebaEurofirms.Infrastructure.Interfaces;
using System.Text.Json;

namespace PruebaEurofirms.Application.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly ApiClientService _apiClientService;
        private IEpisodeRepository _episodeRepository;

        public EpisodeService(ApiClientService apiClientService, IEpisodeRepository episodeRepository)
        {
            _apiClientService = apiClientService;
            _episodeRepository = episodeRepository;
        }

        public async Task<List<Episode>> GetEpisodesAsync()
        {
            var episodes = new List<Episode>();
            var response = await _apiClientService.GetAsync("episode");
            if (response.TryGetProperty("results", out JsonElement resultsElement))
            {
                
                episodes = DeserializeEpisodeResponse(resultsElement);
                _episodeRepository.AddEpisodes(episodes);
            }

            return episodes;
        }
        public async Task<Episode> GetEpisodeAsync(int id)
        {
            var episode = new Episode();
            var response = await _apiClientService.GetAsync($"episode/{id}");
            if (response.TryGetProperty("results", out JsonElement resultsElement))
            {
                episode = DeserializeEpisodeResponse(resultsElement)[0];
            }

            return episode;
        }
        public List<Episode> DeserializeEpisodeResponse(JsonElement response)
        {
            var episodes = new List<Episode>();
            foreach (var episodeJson in response.EnumerateArray())
            {
                var episode = new Episode
                {
                    Id = episodeJson.GetProperty("id").GetInt32(),
                    Name = episodeJson.GetProperty("name").GetString(),
                    Code = episodeJson.GetProperty("episode").GetString(),
                    Url = episodeJson.GetProperty("url").GetString(),
                    AirDate = episodeJson.GetProperty("air_date").GetString(),
                };
                episodes.Add(episode);
            }
            return episodes;
        }

    }
}