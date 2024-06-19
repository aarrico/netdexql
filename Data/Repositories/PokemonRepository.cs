using NetDexQL.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace NetDexQL.Data.Repositories
{
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

        public async Task<Pokemon> AddMon(Pokemon pokemon)
        {
            _context.Pokemon.Add(pokemon);
            await _context.SaveChangesAsync();
            return pokemon;
        }
    }
}