using NetDexQL.Data;
using NetDexQL.Data.Models;

namespace NetDexQL.GraphQL.Types
{
    public class PokemonType : ObjectType<Pokemon>
    {
        protected override void Configure(IObjectTypeDescriptor<Pokemon> descriptor)
        {
            descriptor.Description("Pokémon data");

            // descriptor
            // .Field(p => p.Order)
            // .ResolveWith<Resolvers>(r => r.GetPokemon(default!, default!))
            // .UseDbContext<ApplicationDbContext>()
            // .Description("National Pokémon");
        }

        private class Resolvers
        {
            public Pokemon GetPokemon(Pokemon pokemon, [Service(ServiceKind.Resolver)] ApplicationDbContext dbContext)
            {
                return new Pokemon { Id = Guid.NewGuid(), Name = "MissingNO", BaseExperience = 0, Height = 0, Weight = 0, Order = -1 };
                // for relations
                //return dbContext.Pokemon.Find()
            }
        }
    }
}