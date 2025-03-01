using PruebaEurofirms.Domain.Entities;
using PruebaEurofirms.Application.Interfaces;
using PruebaEurofirms.Infrastructure.Interfaces;
using System.Text.Json;

namespace PruebaEurofirms.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ApiClientService _apiClientService;
        private ICharacterRepository _characterRepository;
        private IEpisodeService _episodeService;

        public CharacterService(ApiClientService apiClientService, ICharacterRepository characterRepository)
        {
            _apiClientService = apiClientService;
            _characterRepository = characterRepository;
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            var characters = new List<Character>();
            var response = await _apiClientService.GetAsync("character");

            if (response.TryGetProperty("results", out JsonElement resultsElement))
            {
                characters = DeserializeCharacterResponse(resultsElement);
                try{
                    _characterRepository.AddCharacters(characters);
                }
                catch{
                    throw new ApplicationException("Se ha producido un error insertando los personajes.");
                }
                
            }

            return characters;
        }
        public List<Character> DeserializeCharacterResponse(JsonElement response)
        {
            var characters = new List<Character>();
            foreach (var characterJson in response.EnumerateArray())
            {
                var character = new Character
                {
                    Id = characterJson.GetProperty("id").GetInt32(),
                    Name = characterJson.GetProperty("name").GetString(),
                    Status = characterJson.GetProperty("status").GetString(),
                    Gender = characterJson.GetProperty("gender").GetString()
                };
                characters.Add(character);
            }
            return characters;
        }
        private List<string> GetEpisodesFromCharacter(JsonElement episodeElement)
        {
            var episodes = new List<string>();

            // Recorremos los episodios (en caso de que sea una lista de URLs)
            foreach (var episode in episodeElement.EnumerateArray())
            {
                episodes.Add(episode.GetString());
            }

            return episodes;
        }

    }
}