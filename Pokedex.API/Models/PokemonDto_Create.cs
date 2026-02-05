using System.ComponentModel.DataAnnotations;

namespace Pokedex.API.Models
{
    public class PokemonDto_Create
    {
        [Required]
        public string Name { get; set; } = "MisingNo";
        [Required]
        public string Type1 { get; set; } = "???";
        public string? Type2 { get; set; }
        public string? Description { get; set; }
    }
}
