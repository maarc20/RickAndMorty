using PruebaEurofirms.Domain.Entities;
using PruebaEurofirms.Application.Interfaces;
using PruebaEurofirms.Infrastructure.Interfaces;
using System.Text.Json;
using System.Data.Entity.ModelConfiguration.Configuration;

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

        public async Task<List<Episode>> GetAllEpisodesAsync()
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

        public async Task<Dictionary<int, List<Episode>>> GetCharactersEpisodesAsync(List<CharacterAPI> charactersAPI)
        {
            var CharactersEpisodes = new Dictionary<int, List<Episode>>{};
            foreach (var characterAPI in charactersAPI){
                var episodes = await GetEpisodesAsync(characterAPI.EpisodeIds);
                CharactersEpisodes.Add(characterAPI.Id, episodes);
            }
            return CharactersEpisodes;
        }

        public async Task<List<Episode>> GetEpisodesAsync(List<int> EpisodeIds)
        {
            string episodeString = string.Join(",", EpisodeIds);
            string endpoint = $"/episode/[{episodeString}]";

            var response = await _apiClientService.GetAsync(endpoint);
            var episodes = DeserializeEpisodeResponse(response);
            return episodes;
        }

        public async Task<Episode> GetEpisodeAsync(string EpisodeUrl)
        {
            var response = await _apiClientService.GetAsync(EpisodeUrl, useBaseAddress: false);
            var episode = DeserializeEpisodeResponse(response)[0];

            return episode;
        }
        private List<Episode> DeserializeEpisodeResponse(JsonElement response)
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

        public void InsertEpisodes(List<Episode> episodes)
        {
            _episodeRepository.AddEpisodes(episodes);
        }

    }
}