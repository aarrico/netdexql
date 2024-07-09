using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HotChocolate.Data.Filters;
using NetDexQL.GraphQL.Types;
using System.Linq.Expressions;

namespace NetDexQL.Data.Models;

public class Pokemon
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "MissingNo";
    public int Height { get; set; }
    public int Weight { get; set; }
    public int? BaseExperience { get; set; }
    public int Order { get; set; }

    public List<PokemonOnType> PokemonTypes { get; set; } = [];
}

public class PokemonType : ObjectType<Pokemon>
{
    protected override void Configure(IObjectTypeDescriptor<Pokemon> descriptor)
    {
        descriptor.Field(p => p.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(p => p.Name).Type<NonNullType<StringType>>();
        descriptor.Field(p => p.PokemonTypes)
            .Type<ListType<PokemonOnTypeType>>();
    }
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

public class PokemonFilterInputType : FilterInputType<Pokemon>
{
    protected override void Configure(IFilterInputTypeDescriptor<Pokemon> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.BindFieldsExplicitly();
        descriptor.Field(p => p.Name);
        descriptor.Field(p => p.Order);
        descriptor.Field(p => p.Height);
        descriptor.Field(p => p.Weight);
        descriptor.Field(p => p.PokemonTypes)
            .Type<ListFilterInputType<PokemonOnTypeFilterInputType>>();
    }
}