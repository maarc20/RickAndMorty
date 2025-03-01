using Microsoft.AspNetCore.Mvc;
using PruebaEurofirms.Application.Interfaces;

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

        // Acción para obtener todos los personajes
        [HttpPost("characters")]
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

        [HttpGet("episodes")]
        public async Task<IActionResult> GetAllEpisodes()
        {
            var episodes = await _episodeService.GetAllEpisodesAsync();  // Llama al servicio mediante la interfaz
            return Ok(episodes);  // Responde con los personajes en formato JSON
        }

    }

}

