using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetDexQL.Data.Models;

public class Pokemon
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public  int Height { get; set; }
    public int Weight { get; set; }
    public int? BaseExperience { get; set; }
    public int Order { get; set; }

    public List<MonType> MonTypes { get; } = [];
    public List<PokemonOnType> PokemonTypes { get; } = [];
}

public class PokemonConfiguration : IEntityTypeConfiguration<Pokemon>
{
    public void Configure(EntityTypeBuilder<Pokemon> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Name).IsUnique();

        builder
            .Property(p => p.Name)
            .HasMaxLength(50);
    }
}