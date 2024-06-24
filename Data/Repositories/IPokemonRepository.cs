using NetDexQL.Data.Models;

namespace NetDexQL.Data.Repositories;

public interface IPokemonRepository
{
    Task<int> GetMonCount();
    Task<List<Pokemon>> GetAllMons();
    List<Pokemon> GetMonsByTypeId(Guid typeId);
    List<Pokemon> GetMonsByTypeName(string typeName);
}