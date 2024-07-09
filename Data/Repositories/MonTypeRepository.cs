using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;

namespace NetDexQL.Data.Repositories;

public class MonTypeRepository : IMonTypeRepository
{
    private readonly ApplicationDbContext _context;

    public MonTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MonType>> GetAllTypes()
    {
        return await _context.MonTypes.ToListAsync();
    }

    public async Task<MonType> GetTypeByName(string name)
    {
        return await _context.MonTypes.FirstAsync(t => t.Name == name);
    }
}