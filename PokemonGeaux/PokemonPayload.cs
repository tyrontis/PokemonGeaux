using Newtonsoft.Json;

namespace PokemonGeaux.Models
{
    public class PokemonPayload
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<PokemonType>? Types { get; set; }

        [JsonProperty("damage_relations")]
        public DamageRelations? DamageRelations { get; set; }
    }

    public class PokemonType
    {
        public Type? Type { get; set; }
    }

    public class Type
    {
        public string? Name { get; set; }
    }

    public class DamageRelations
    {
        [JsonProperty("double_damage_to")]
        public List<DoubleDamageTo>? DoubleDamageTo { get; set; }

        [JsonProperty("double_damage_from")]
        public List<DoubleDamageFrom>? DoubleDamageFrom { get; set; }
    }

    public class DoubleDamageTo
    {
        public string? Name { get; set; }
    }

    public class DoubleDamageFrom
    {
        public string? Name { get; set; }
    }
}
