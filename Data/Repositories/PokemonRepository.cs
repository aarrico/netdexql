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
}