using Microsoft.EntityFrameworkCore;
using Pokedex.API.Entities;

namespace Pokedex.API.Contexts
{
    public class PokedexContext : DbContext
    {
        public DbSet<Pokemon> Pokemons { get; set; }

        public PokedexContext(DbContextOptions<PokedexContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pokemon>().HasData(
                new Pokemon("Bulbasaur", "grass")
                {
                    Id = 1,
                    Type2 = "poison",
                    Description = "bulba boy",
                },
                new Pokemon("Ivysaur", "grass")
                {
                    Id = 2,
                    Type2 = "poison",
                    Description = "big bulba",
                }, new Pokemon("Venusaur", "grass")
                {
                    Id = 3,
                    Type2 = "poison",
                    Description = "bulba chungus",
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
