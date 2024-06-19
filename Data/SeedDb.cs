using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;
using NetDexQL.Data.Repositories;

namespace NetDexQL.Data
{
    public static class SeedDb
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var client = new HttpClient();
            var pokeRepo = new PokemonRepository(context);

            var json = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
            var totalMons = 1025;
            var count = await pokeRepo.GetMonCount();

            Console.WriteLine($"{count}/{totalMons} mons populated");

            while (count < totalMons)
            {
                count++;
                Console.WriteLine($"Adding new pokemon {count}");
                var newMon = await GetNewPokemon($"https://pokeapi.co/api/v2/pokemon/{count}", client);
                await pokeRepo.AddMon(newMon);
            }
        }

        private async static Task<Pokemon> GetNewPokemon(string resourceUrl, HttpClient client)
        {
            var json = await client.GetStringAsync(resourceUrl);
            return JsonSerializer.Deserialize<Pokemon>(json);
        }
    }
}