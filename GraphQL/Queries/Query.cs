using NetDexQL.Data;
using NetDexQL.Data.Models;
using NetDexQL.Data.Repositories;

namespace NetDexQL.GraphQL.Queries;

public class Query
{
    public IQueryable<Pokemon> GetMons([Service] ApplicationDbContext dbContext) => dbContext.Pokemon;

    public IQueryable<MonType> GetMonTypes([Service] ApplicationDbContext dbContext) => dbContext.MonTypes;
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field(q => q.GetMons(default))
            .UseFiltering<PokemonFilterInputType>();

        descriptor
            .Field(q => q.GetMonTypes(default))
            .UseFiltering<MonType>();
    }
}