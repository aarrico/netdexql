using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetDexQL.Data.Models;

public class PokemonOnType
{
    public Guid PokemonId { get; set; }

    public Guid MonTypeId { get; set; }
    
    public bool IsPrimary { get; set; }
}

public class PokemonOnTypeConfiguration : IEntityTypeConfiguration<PokemonOnType>
{
    public void Configure(EntityTypeBuilder<PokemonOnType> builder)
    {
        builder.HasKey(p => new { p.PokemonId, p.MonTypeId });
    }
}