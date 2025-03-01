using PruebaEurofirms.Domain.Entities;
using PruebaEurofirms.Application.Interfaces;
using PruebaEurofirms.Infrastructure.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PruebaEurofirms.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ApiClientService _apiClientService;
        private ICharacterRepository _characterRepository;
        private IEpisodeService _episodeService;
        private ICharacterEpisodeRepository _characterEpisodeRepository;

        public CharacterService(ApiClientService apiClientService,
         ICharacterRepository characterRepository,
         IEpisodeService episodeService,
         ICharacterEpisodeRepository characterEpisodeRepository)
        {
            _apiClientService = apiClientService;
            _characterRepository = characterRepository;
            _episodeService = episodeService;
            _characterEpisodeRepository = characterEpisodeRepository;
        }

        public async Task<List<CharacterAPI>> GetAllCharactersAsync()
        {
            var charactersAPI = new List<CharacterAPI>();
            var response = await _apiClientService.GetAsync("character");

            if (response.TryGetProperty("results", out JsonElement resultsElement))
            {
                try{
                    // Inserting characters
                    charactersAPI = DeserializeCharacterResponse(resultsElement);
                    _characterRepository.AddCharacters(charactersAPI);
                    // Retrieving Episodes from all Characters
                    var CharactersEpisodes = await _episodeService.GetCharactersEpisodesAsync(charactersAPI);
                    // Inserting Epiosdes
                    var episodesSet = new HashSet<Episode>();
                    foreach (var kvp in CharactersEpisodes){
                        episodesSet.UnionWith(kvp.Value);
                    }
                    List<Episode> episodes = new List<Episode>(episodesSet);
                    _episodeService.InsertEpisodes(episodes);
                    //Inserting Character Episodes
                    
                    _characterEpisodeRepository.AddCharacterEpisodes(CharactersEpisodes);
                }
                catch{
                    throw new ApplicationException("Se ha producido un error insertando los personajes.");
                }
                
            }

            return charactersAPI;
        }
        private List<CharacterAPI> DeserializeCharacterResponse(JsonElement response)
        {
            var characters = new List<CharacterAPI>();
            foreach (var characterJson in response.EnumerateArray())
            {
                var character = new CharacterAPI
                {
                    Id = characterJson.GetProperty("id").GetInt32(),
                    Name = characterJson.GetProperty("name").GetString(),
                    Status = characterJson.GetProperty("status").GetString(),
                    Gender = characterJson.GetProperty("gender").GetString(),
                    EpisodeIds = GetEpisodeIdsFromCharacter(characterJson.GetProperty("episode"))
                };
                characters.Add(character);
            }
            return characters;
        }
        private List<string> GetEpisodesFromCharacter(JsonElement episodeElement)
        {
            var episodes = new List<string>();

            foreach (var episode in episodeElement.EnumerateArray())
            {
                episodes.Add(episode.GetString());
            }

            return episodes;
        }
                
        private List<int> GetEpisodeIdsFromCharacter(JsonElement episodeElement)
        {
            var episodes = new List<int>();


            foreach (var episode in episodeElement.EnumerateArray())
            {
                Match match = Regex.Match(episode.GetString(), @".*/episode/(\d+)$");
                if (match.Success)
                {
                    episodes.Add(int.Parse(match.Groups[1].Value));
                }
            }

            return episodes;
        }

        public async Task<List<Character>> GetCharactersByStatusAsync(Status status)
        {
            var characters = _characterRepository.GetCharactersFiltered(status);
            return characters;
        }

        public Boolean DeleteCharacter(int CharacterId)
        {
            var characterDeleted = _characterRepository.DeleteCharacter(CharacterId);
            var EpisodeIds = _characterEpisodeRepository.GetEpisodeIdsFromCharacterId(CharacterId);
            _characterEpisodeRepository.DeleteEpisodesFromCaracterId(CharacterId);
            _episodeService.DeleteEpisodes(EpisodeIds);
            return characterDeleted;
        }
    }
}