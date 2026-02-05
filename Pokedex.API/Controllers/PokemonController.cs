using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Models;

namespace Pokedex.API.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        PokemonDataStore pokemonDataStore;
        ILogger<PokemonController> logger;

        public PokemonController(PokemonDataStore pokemonDataStore,
            ILogger<PokemonController> logger)
        {
            this.pokemonDataStore = pokemonDataStore;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PokemonDto>> GetAllPokemon()
        {
            return Ok(pokemonDataStore.PokemonData);
        }

        [HttpGet("{pokemonId}", Name = "GetPokemon")]
        public ActionResult<PokemonDto> GetPokemon(int pokemonId)
        {
            try
            {
                var pokemon = pokemonDataStore.PokemonData.FirstOrDefault(
                    pok => pok.Id == pokemonId);

                if (pokemon == null)
                {
                    return NotFound();
                }

                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                logger.LogCritical(
                    $"Exception while getting a pokemon with id {pokemonId}", ex);

                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpPost]
        public ActionResult<PokemonDto> CreatePokemon([FromBody] PokemonDto_Create pokemonToCreate)
        {
            int maxPokemonId = pokemonDataStore.PokemonData.Max(pokemon => pokemon.Id);

            var newPokemon = new PokemonDto()
            {
                Id = ++maxPokemonId,
                Name = pokemonToCreate.Name,
                Type1 = pokemonToCreate.Type1,
                Type2 = pokemonToCreate.Type2,
                Description = pokemonToCreate.Description,
            };

            pokemonDataStore.PokemonData.Add(newPokemon);

            return CreatedAtRoute("GetPokemon",
                new
                {
                    pokemonId = maxPokemonId,
                },
                newPokemon);
        }

        [HttpPut("{pokemonId}")]
        public ActionResult UpdatePokemon(int pokemonId, PokemonDto_Update pokemon)
        {
            var pokemonFromStore = pokemonDataStore.PokemonData.FirstOrDefault(
                pok => pok.Id == pokemonId);

            if (pokemonFromStore == null) return NotFound();

            pokemonFromStore.Name = pokemon.Name;
            pokemonFromStore.Type1 = pokemon.Type1;
            pokemonFromStore.Type2 = pokemon.Type2;
            pokemonFromStore.Description = pokemon.Description;

            return NoContent();
        }

        [HttpPatch("{pokemonId}")]
        public ActionResult PatchPokemon(int pokemonId, 
            JsonPatchDocument<PokemonDto_Update> patchDocument)
        {
            var pokemonFromStore = pokemonDataStore.PokemonData.FirstOrDefault(
                pok => pok.Id == pokemonId);

            if (pokemonFromStore == null) return NotFound();

            var pokemonToPatch =
                new PokemonDto_Update()
                {
                    Name = pokemonFromStore.Name,
                    Type1 = pokemonFromStore.Type1,
                    Type2 = pokemonFromStore.Type2,
                    Description = pokemonFromStore.Description,
                };

            patchDocument.ApplyTo(pokemonToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if(!TryValidateModel(pokemonToPatch)) return BadRequest(ModelState);

            pokemonFromStore.Name = pokemonToPatch.Name;
            pokemonFromStore.Type1 = pokemonToPatch.Type1;
            pokemonFromStore.Type2 = pokemonToPatch.Type2;
            pokemonFromStore.Description = pokemonToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        public ActionResult DeletePokemon(int pokemonId)
        {
            var pokemonFromStore = pokemonDataStore.PokemonData.FirstOrDefault(
    pok => pok.Id == pokemonId);

            if (pokemonFromStore == null) return NotFound();

            pokemonDataStore.PokemonData.Remove(pokemonFromStore);

            return NoContent();
        }
    }
}
