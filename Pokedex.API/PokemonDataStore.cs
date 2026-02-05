using Pokedex.API.Models;

namespace Pokedex.API
{
    public class PokemonDataStore
    {
        public List<PokemonDto> PokemonData;

        public PokemonDataStore()
        {
            PokemonData = new List<PokemonDto>()
            {
                new PokemonDto()
                {
                    Id = 1,
                    Name = "bulbasaur",
                    Type1 = "grass",
                    Type2 = "poison",
                    Description = "For some time after its birth, it uses the nutrients that are packed into the seed on its back in order to grow."
                },
                new PokemonDto()
                {
                    Id = 2,
                    Name = "ivysaur",
                    Type1 = "grass",
                    Type2 = "poison",
                    Description = "The more sunlight Ivysaur bathes in, the more strength wells up within it, allowing the bud on its back to grow larger."
                },
                new PokemonDto()
                {
                    Id = 3,
                    Name = "venusaur",
                    Type1 = "grass",
                    Type2 = "poison",
                    Description = "While it basks in the sun, it can convert the light into energy. As a result, it is more powerful in the summertime."
                },
                new PokemonDto()
                {
                    Id = 4,
                    Name = "charmander",
                    Type1 = "grass",
                    Type2 = "poison",
                    Description = "The flame on its tail shows the strength of its life-force. If Charmander is weak, the flame also burns weakly."
                },
                new PokemonDto()
                {
                    Id = 5,
                    Name = "charmeleon",
                    Type1 = "fire",
                    Description = "When it swings its burning tail, the temperature around it rises higher and higher, tormenting its opponents."
                },
                new PokemonDto()
                {
                    Id = 6,
                    Name = "charizard",
                    Type1 = "fire",
                    Type2 = "flying",
                    Description = "If Charizard becomes truly angered, the flame at the tip of its tail burns in a light blue shade."
                },
                new PokemonDto()
                {
                    Id = 7,
                    Name = "squirtle",
                    Type1 = "water",
                    Description = "When it retracts its long neck into its shell, it squirts out water with vigorous force."
                },
                new PokemonDto()
                {
                    Id = 8,
                    Name = "wartortle",
                    Type1 = "water",
                    Description = "It is recognized as a symbol of longevity. If its shell has algae on it, that Wartortle is very old."
                },
                new PokemonDto()
                {
                    Id = 9,
                    Name = "blastoise",
                    Type1 = "water",
                    Description = "It crushes its foe under its heavy body to cause fainting. In a pinch, it will withdraw inside its shell."
                }
            };
        }
    }
}
