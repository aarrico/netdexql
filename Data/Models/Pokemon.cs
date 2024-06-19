using System.Text.Json.Serialization;

namespace NetDexQL.Data.Models
{
    public class Pokemon
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [property: JsonPropertyName("name")] public required string Name { get; set; }
        [property: JsonPropertyName("height")] public required int Height { get; set; }
        [property: JsonPropertyName("weight")] public required int Weight { get; set; }
        [property: JsonPropertyName("base_experience")] public required int? BaseExperience { get; set; }
        [property: JsonPropertyName("order")] public required int Order { get; set; }
    }
}