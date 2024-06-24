using NetDexQL.Data.Models;
using NetDexQL.Data.Repositories;

namespace NetDexQL.GraphQL.Queries;

public class Query
{
    public async Task<List<Pokemon>> GetPokemon([Service] IPokemonRepository pokemonRepository) =>
        await pokemonRepository.GetAllMons();

    public List<Pokemon> GetMonsByTypeId([Service] IPokemonRepository pokemonRepository, Guid typeId) =>
        pokemonRepository.GetMonsByTypeId(typeId);

    public List<Pokemon> GetMonsByTypeName([Service] IPokemonRepository pokemonRepository, string typeName) =>
        pokemonRepository.GetMonsByTypeName(typeName);
}