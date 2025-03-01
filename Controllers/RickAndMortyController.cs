using Microsoft.AspNetCore.Mvc;
using PruebaEurofirms.Application.Interfaces;
using PruebaEurofirms.Domain.Entities;

namespace PruebaEurofirms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RickAndMortyController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        private readonly IEpisodeService _episodeService;

        public RickAndMortyController(ICharacterService characterService, IEpisodeService episodeService)
        {
            _characterService = characterService;
            _episodeService = episodeService;
        }

        // Endpoint para insertar todos los personajes en la BBDD
        [HttpPost("UpdateCharacters")]
        public async Task<IActionResult> GetAllCharacters()
        {
            try
            {
                var characters = await _characterService.GetAllCharactersAsync();  // Llama al servicio mediante la interfaz
                var response = new 
                {
                    Message = $"Personajes importados correctamente: {characters.Count}",
                    Characters = characters
                };
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // Endpoint para obtener todos los episodios
        [HttpPost("UpdateEpisodes")]
        public async Task<IActionResult> GetAllEpisodes()
        {
            var episodes = await _episodeService.GetAllEpisodesAsync();  // Llama al servicio mediante la interfaz
            return Ok(episodes);  // Responde con los personajes en formato JSON
        }

        // Endpoint para obtener los personajes filtrados por Status
        [HttpGet("GetCharactersByStatus")]
        public async Task<IActionResult> GetCharactersByStatusAsync(Status status)
        {
            var characters = await _characterService.GetCharactersByStatusAsync(status); 
            
            var response = new 
            {
                NumberOfCharacters = characters.Count,
                Characters = characters
            };
            return Ok(response);  
        }

        [HttpDelete("DeleteCharacterById")]
        public async Task<IActionResult> DeleteCharacter(int CharacterId)
        {
            var character_deleted = _characterService.DeleteCharacter(CharacterId);
            var response = String.Empty;
            if (character_deleted){
                response = $"Character {CharacterId} deleted.";
            }
            else
            {
                response = $"Character {CharacterId} not found.";
            }
            return Ok(response);  
        }



    }

}

