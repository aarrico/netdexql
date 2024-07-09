using NetDexQL.Data.Models;

namespace NetDexQL.Data.Repositories;

public interface IPokemonRepository
{
    Task<int> GetMonCount();
    Task<List<Pokemon>> GetAllMons();
}