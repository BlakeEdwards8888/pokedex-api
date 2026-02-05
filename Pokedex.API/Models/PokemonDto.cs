namespace Pokedex.API.Models
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "MisingNo";
        public string Type1 { get; set; } = "???";
        public string? Type2 { get; set; }
        public string? Description { get; set; }
    }
}
