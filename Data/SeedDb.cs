using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetDexQL.Data;

public static class SeedDb
{
    private static readonly MonType[] TypeList =
    [
        new MonType { Name = "normal" },
        new MonType { Name = "water" },
        new MonType { Name = "fire" },
        new MonType { Name = "grass" },
        new MonType { Name = "psychic" },
        new MonType { Name = "dark" },
        new MonType { Name = "ghost" },
        new MonType { Name = "steel" },
        new MonType { Name = "fairy" },
        new MonType { Name = "electric" },
        new MonType { Name = "poison" },
        new MonType { Name = "bug" },
        new MonType { Name = "fighting" },
        new MonType { Name = "ice" },
        new MonType { Name = "ground" },
        new MonType { Name = "rock" },
        new MonType { Name = "flying" },
        new MonType { Name = "dragon" }
    ];

    private static readonly HttpClient Client = new();

    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        var typeList = TypeList;
        if (!context.Types.Any())
        {
            PopulateTypeTable(context);
        }
        else
        {
            typeList = context.Types.ToArray();
        }

        await PopulatePokemonTable(context, typeList);
    }

    private static void PopulateTypeTable(ApplicationDbContext context)
    {
        context.Types.AddRange(TypeList);
        context.SaveChanges();
    }

    private static async Task PopulatePokemonTable(ApplicationDbContext context, MonType[] typeList)
    {
        const string apiUrl = "https://pokeapi.co/api/v2/pokemon?limit=5000";

        var res = await Client.GetStringAsync(apiUrl);
        var allMons = JObject.Parse(res)["results"] as JArray;

        if (allMons == null)
        {
            Console.WriteLine("Something went wrong contacting pokéapi, exiting seed!");
            Environment.Exit(-1302);
        }

        var dbCount = context.Pokemon.Count();

        while (dbCount < allMons.Count)
        {
            try
            {
                var monUrl = (string)allMons[dbCount]["url"]!;
                AddMon(await Client.GetStringAsync(monUrl), typeList, context);
                dbCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    private static async void AddMon(string rawMonData, MonType[] typeList, ApplicationDbContext context)
    {
        var newMonRaw = JObject.Parse(rawMonData);
        var newMon = CreateMonFromApi(newMonRaw);
        
        context.Pokemon.Add(newMon);

        var types = newMonRaw["types"] as JArray;

        foreach (var type in types!)
        {
            var typeName = (string)type["type"]!["name"]!;
            var typeSlot = (int)type["slot"]!;
            var typeId = typeList.First(t => t.Name == typeName).Id;

            await context.PokemonOnTypes.AddAsync(
                new PokemonOnType
                {
                    PokemonId = newMon.Id,
                    MonTypeId = typeId,
                    IsPrimary = typeSlot == 1
                });
        }

        await context.SaveChangesAsync();
    }

    private static Pokemon CreateMonFromApi(JObject newMonRaw)
    {
        return new Pokemon
        {
            Name = (string)newMonRaw["name"],
            Height = newMonRaw["height"].Type == JTokenType.Null ? 0 : (int)newMonRaw["height"],
            Weight = newMonRaw["weight"].Type == JTokenType.Null ? 0 : (int)newMonRaw["weight"],
            BaseExperience = newMonRaw["base_experience"].Type == JTokenType.Null
                ? 0
                : (int)newMonRaw["base_experience"],
            Order = newMonRaw["order"].Type == JTokenType.Null ? -1 : (int)newMonRaw["order"]
        };
    }
}