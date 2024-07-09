using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotChocolate.Data.Filters;

namespace NetDexQL.Data.Models;

public class PokemonOnType
{
    public Guid PokemonId { get; set; }
    public Pokemon Pokemon { get; set; }

    public Guid MonTypeId { get; set; }
    public MonType MonType { get; set; }
    
    public bool IsPrimary { get; set; }
}

public class PokemonOnTypeType : ObjectType<PokemonOnType>
{
    protected override void Configure(IObjectTypeDescriptor<PokemonOnType> descriptor)
        {
            descriptor.Field(pt => pt.MonType).Type<MonTypeType>();
            descriptor.Field(pt => pt.IsPrimary);
        }
}

public class PokemonOnTypeConfiguration : IEntityTypeConfiguration<PokemonOnType>
{
    public void Configure(EntityTypeBuilder<PokemonOnType> builder)
    {
        builder.HasKey(p => new { p.PokemonId, p.MonTypeId });
    }
}

public class PokemonOnTypeFilterInputType : FilterInputType<PokemonOnType>
{
    protected override void Configure(IFilterInputTypeDescriptor<PokemonOnType> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(pt => pt.MonType.Name).Name("type");
        descriptor.Field(pt => pt.IsPrimary);
    }
}
