using NetDexQL.Data.Models;
using NetDexQL.Data.Repositories;

namespace NetDexQL.GraphQL.Queries;

public class Query
{
    public async Task<List<Pokemon>> GetPokemon([Service] IPokemonRepository pokemonRepository) =>
        await pokemonRepository.GetAllMons();

    public List<Pokemon> GetMonsForType([Service] IPokemonRepository pokemonRepository, Guid typeId) =>
        pokemonRepository.GetMonsForType(typeId);
}