using NetDexQL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace NetDexQL.Data.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly ApplicationDbContext _context;

    public PokemonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetMonCount()
    {
        return await _context.Pokemon.CountAsync();
    }

    public async Task<List<Pokemon>> GetAllMons()
    {
        return await _context.Pokemon.ToListAsync();
    }

    public async Task<Pokemon?> GetMonById(Guid id)
    {
        return await _context.Pokemon.FindAsync(id);
    }

    public List<Pokemon> GetMonsForType(Guid typeId)
    {
        return _context.Types
            .Include(monType => monType.Pokemon)
            .FirstOrDefault(t => t.Id == typeId)
            ?.Pokemon
            .OrderBy(p => p.Order)
            .ToList() ?? [];
    }
}