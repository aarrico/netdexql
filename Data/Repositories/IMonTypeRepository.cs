using NetDexQL.Data.Models;

namespace NetDexQL.Data.Repositories;

public interface IMonTypeRepository
{
    Task<List<MonType>> GetAllTypes();
    Task<MonType> GetTypeByName(string name);
}