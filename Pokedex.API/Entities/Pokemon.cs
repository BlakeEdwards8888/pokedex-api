using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokedex.API.Entities
{
    public class Pokemon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type1 { get; set; }
        public string? Type2 { get; set; }
        public string? Description { get; set; }

        public Pokemon(string name, string type1)
        {
            Name = name;
            Type1 = type1;
        }
    }
}
