using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Models;

namespace Pokedex.API.Controllers
{
    [Route("api/pokemon")]
    [ApiController]
    [Produces("application/json")]
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

        /// <summary>
        /// Returns all pokemon currently registered in the pokedex
        /// </summary>
        /// <returns>A detailed list of each pokemon currently registered in the pokedex</returns>
        /// <response code = "200">Returns the list of pokemon</response>
        [HttpGet]
        public ActionResult<IEnumerable<PokemonDto>> GetAllPokemon()
        {
            return Ok(pokemonDataStore.PokemonData);
        }

        /// <summary>
        /// Returns data for a single specified pokemon
        /// </summary>
        /// <param name="pokemonId">The ID number with which the desired pokemon was registered</param>
        /// <returns>Detailed information about the requested pokemon</returns>
        /// <response code = "404">No pokemon with the given ID was found</response>
        /// <response code = "200">Returns the specified pokemon</response>
        /// <response code = "500">An exception occured while getting a pokemon with the specified ID</response>
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

        /// <summary>
        /// Adds a new pokemon to the pokedex. Add your favorite!
        /// </summary>
        /// <param name="pokemonToCreate">Json format object containing the pokemon's name, type(s), and description</param>
        /// <response code = "204">Your pokemon was successfully registered in the pokedex</response>
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

        /// <summary>
        /// Updates one of the currently registered pokemon
        /// </summary>
        /// <param name="pokemonId">The ID number with which the desired pokemon was registered</param>
        /// <param name="pokemon">Json format object containing the pokemon's name, type(s), and description</param>
        /// <response code = "404">No pokemon with the given ID was found</response>
        /// <response code = "201">The pokemon you requested was successfully updated</response>
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

        /// <summary>
        /// Partially updates one of the registered pokemon
        /// </summary>
        /// <param name="pokemonId">The ID number with which the desired pokemon was registered</param>
        /// <param name="patchDocument">A Json Patch Document containing details for the patch</param>
        /// <response code = "404">No pokemon with the given ID was found</response>
        /// <response code = "400">The model state is invalid</response>
        /// <response code = "201">The pokemon you requested was successfully patched</response>
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

        /// <summary>
        /// Removes a pokemon from the pokedex
        /// </summary>
        /// <param name="pokemonId">The ID number with which the desired pokemon was registered</param>
        /// <response code = "404">No pokemon with the given ID was found</response>
        /// <response code = "201">The pokemon you requested was successfully deleted</response>
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
