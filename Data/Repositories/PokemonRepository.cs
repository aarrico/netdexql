using NetDexQL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace NetDexQL.Data.Repositories;

public class PokemonRepository(ApplicationDbContext context) : IPokemonRepository
{
    public async Task<int> GetMonCount()
    {
        return await context.Pokemon.CountAsync();
    }

    public async Task<List<Pokemon>> GetAllMons()
    {
        return await context.Pokemon.ToListAsync();
    }

    public async Task<Pokemon?> GetMonById(Guid id)
    {
        return await context.Pokemon.FindAsync(id);
    }

    public List<Pokemon> GetMonsByTypeId(Guid typeId)
    {
        return context.Types
            .Include(monType => monType.Pokemon)
            .FirstOrDefault(t => t.Id == typeId)
            ?.Pokemon
            .OrderBy(p => p.Order)
            .ToList() ?? [];
    }

    public List<Pokemon> GetMonsByTypeName(string typeName)
    {
        return context.Types.Include(mt => mt.Pokemon)
            .FirstOrDefault(t => t.Name == typeName)
            ?.Pokemon
            .OrderBy(p => p.Order)
            .ToList() ?? [];
    }
}