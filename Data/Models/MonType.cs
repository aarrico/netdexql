using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetDexQL.Data.Models;

public class MonType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }

    public List<Pokemon> Pokemon { get; } = [];
    public List<PokemonOnType> PokemonTypes { get; } = [];
}
    
public class TypeConfiguration : IEntityTypeConfiguration<MonType>
{
    public void Configure(EntityTypeBuilder<MonType> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(t => t.Name);
        
        builder
            .Property(p => p.Name)
            .HasMaxLength(50);

        builder
            .HasMany(t => t.Pokemon)
            .WithMany(t => t.MonTypes)
            .UsingEntity<PokemonOnType>();
    }
}