using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;
using Newtonsoft.Json.Linq;

namespace NetDexQL.Data;

public static class SeedDb
{
    private static readonly HttpClient Client = new();

    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        var typeIds = PopulateTypeTable(context);
        PopulateTypeEffectivenessTable(typeIds, context);
        
        await PopulatePokemonTable(context, typeIds);
    }

    private static Dictionary<string, Guid> PopulateTypeTable(ApplicationDbContext context)
    {
        if (!context.MonTypes.Any())
        {
            var typeList = new[]
            {
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
            };

            context.MonTypes.AddRange(typeList);
            context.SaveChanges();
        }

        return context.MonTypes.ToDictionary(t => t.Name, t => t.Id);
    }

    private static async Task PopulatePokemonTable(ApplicationDbContext context, Dictionary<string, Guid> typeIds)
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
                AddMon(await Client.GetStringAsync(monUrl), typeIds, context);
                dbCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    private static void AddMon(string rawMonData, Dictionary<string, Guid> typeIds,
        ApplicationDbContext context)
    {
        var newMonRaw = JObject.Parse(rawMonData);
        var newMon = CreateMonFromApi(newMonRaw);

        context.Pokemon.Add(newMon);

        var types = newMonRaw["types"] as JArray;

        foreach (var type in types!)
        {
            var typeName = (string)type["type"]!["name"]!;
            var isPriamryType = (int)type["slot"]! == 1;
            var typeId = typeIds[typeName];

            context.PokemonOnTypes.Add(
                new PokemonOnType
                {
                    PokemonId = newMon.Id,
                    MonTypeId = typeId,
                    IsPrimary = isPriamryType
                });
        }

        context.SaveChanges();
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

    private static void PopulateTypeEffectivenessTable(Dictionary<string, Guid> typeIds, ApplicationDbContext context)
    {
        if (context.TypeEffectivenesses.Any())
        {
            return;
        }
        
        var allEffectivenesses = new List<TypeEffectiveness>
        {
            // NORMAL
            new(typeIds["normal"], typeIds["normal"], 1.0),
            new(typeIds["normal"], typeIds["water"], 1.0),
            new(typeIds["normal"], typeIds["fire"], 1.0),
            new(typeIds["normal"], typeIds["grass"], 1.0),
            new(typeIds["normal"], typeIds["psychic"], 1.0),
            new(typeIds["normal"], typeIds["dark"], 1.0),
            new(typeIds["normal"], typeIds["ghost"], 0.0),
            new(typeIds["normal"], typeIds["steel"], 0.5),
            new(typeIds["normal"], typeIds["fairy"], 1.0),
            new(typeIds["normal"], typeIds["electric"], 1.0),
            new(typeIds["normal"], typeIds["poison"], 1.0),
            new(typeIds["normal"], typeIds["bug"], 1.0),
            new(typeIds["normal"], typeIds["fighting"], 1.0),
            new(typeIds["normal"], typeIds["ice"], 1.0),
            new(typeIds["normal"], typeIds["ground"], 1.0),
            new(typeIds["normal"], typeIds["rock"], 0.5),
            new(typeIds["normal"], typeIds["flying"], 1.0),
            new(typeIds["normal"], typeIds["dragon"], 1.0),

            // WATER
            new(typeIds["water"], typeIds["normal"], 1.0),
            new(typeIds["water"], typeIds["water"], 0.5),
            new(typeIds["water"], typeIds["fire"], 2.0),
            new(typeIds["water"], typeIds["grass"], 0.5),
            new(typeIds["water"], typeIds["psychic"], 1.0),
            new(typeIds["water"], typeIds["dark"], 1.0),
            new(typeIds["water"], typeIds["ghost"], 1.0),
            new(typeIds["water"], typeIds["steel"], 1.0),
            new(typeIds["water"], typeIds["fairy"], 1.0),
            new(typeIds["water"], typeIds["electric"], 1.0),
            new(typeIds["water"], typeIds["poison"], 1.0),
            new(typeIds["water"], typeIds["bug"], 1.0),
            new(typeIds["water"], typeIds["fighting"], 1.0),
            new(typeIds["water"], typeIds["ice"], 1.0),
            new(typeIds["water"], typeIds["ground"], 2.0),
            new(typeIds["water"], typeIds["rock"], 2.0),
            new(typeIds["water"], typeIds["flying"], 1.0),
            new(typeIds["water"], typeIds["dragon"], 0.5),

            // FIRE
            new(typeIds["fire"], typeIds["normal"], 1.0),
            new(typeIds["fire"], typeIds["water"], 0.5),
            new(typeIds["fire"], typeIds["fire"], 0.5),
            new(typeIds["fire"], typeIds["grass"], 2.0),
            new(typeIds["fire"], typeIds["psychic"], 1.0),
            new(typeIds["fire"], typeIds["dark"], 1.0),
            new(typeIds["fire"], typeIds["ghost"], 1.0),
            new(typeIds["fire"], typeIds["steel"], 2.0),
            new(typeIds["fire"], typeIds["fairy"], 1.0),
            new(typeIds["fire"], typeIds["electric"], 1.0),
            new(typeIds["fire"], typeIds["poison"], 1.0),
            new(typeIds["fire"], typeIds["bug"], 2.0),
            new(typeIds["fire"], typeIds["fighting"], 1.0),
            new(typeIds["fire"], typeIds["ice"], 2.0),
            new(typeIds["fire"], typeIds["ground"], 1.0),
            new(typeIds["fire"], typeIds["rock"], 0.5),
            new(typeIds["fire"], typeIds["flying"], 1.0),
            new(typeIds["fire"], typeIds["dragon"], 0.5),

            // GRASS
            new(typeIds["grass"], typeIds["normal"], 1.0),
            new(typeIds["grass"], typeIds["water"], 2.0),
            new(typeIds["grass"], typeIds["fire"], 0.5),
            new(typeIds["grass"], typeIds["grass"], 0.5),
            new(typeIds["grass"], typeIds["psychic"], 1.0),
            new(typeIds["grass"], typeIds["dark"], 1.0),
            new(typeIds["grass"], typeIds["ghost"], 1.0),
            new(typeIds["grass"], typeIds["steel"], 0.5),
            new(typeIds["grass"], typeIds["fairy"], 1.0),
            new(typeIds["grass"], typeIds["electric"], 1.0),
            new(typeIds["grass"], typeIds["poison"], 0.5),
            new(typeIds["grass"], typeIds["bug"], 0.5),
            new(typeIds["grass"], typeIds["fighting"], 1.0),
            new(typeIds["grass"], typeIds["ice"], 1.0),
            new(typeIds["grass"], typeIds["ground"], 2.0),
            new(typeIds["grass"], typeIds["rock"], 2.0),
            new(typeIds["grass"], typeIds["flying"], 0.5),
            new(typeIds["grass"], typeIds["dragon"], 0.5),

            // PSYCHIC
            new(typeIds["psychic"], typeIds["normal"], 1.0),
            new(typeIds["psychic"], typeIds["water"], 1.0),
            new(typeIds["psychic"], typeIds["fire"], 1.0),
            new(typeIds["psychic"], typeIds["grass"], 1.0),
            new(typeIds["psychic"], typeIds["psychic"], 0.5),
            new(typeIds["psychic"], typeIds["dark"], 0.0),
            new(typeIds["psychic"], typeIds["ghost"], 1.0),
            new(typeIds["psychic"], typeIds["steel"], 0.5),
            new(typeIds["psychic"], typeIds["fairy"], 1.0),
            new(typeIds["psychic"], typeIds["electric"], 1.0),
            new(typeIds["psychic"], typeIds["poison"], 2.0),
            new(typeIds["psychic"], typeIds["bug"], 1.0),
            new(typeIds["psychic"], typeIds["fighting"], 2.0),
            new(typeIds["psychic"], typeIds["ice"], 1.0),
            new(typeIds["psychic"], typeIds["ground"], 1.0),
            new(typeIds["psychic"], typeIds["rock"], 1.0),
            new(typeIds["psychic"], typeIds["flying"], 1.0),
            new(typeIds["psychic"], typeIds["dragon"], 1.0),

            // --DARK
            new(typeIds["dark"], typeIds["normal"], 1.0),
            new(typeIds["dark"], typeIds["water"], 1.0),
            new(typeIds["dark"], typeIds["fire"], 1.0),
            new(typeIds["dark"], typeIds["grass"], 1.0),
            new(typeIds["dark"], typeIds["psychic"], 2.0),
            new(typeIds["dark"], typeIds["dark"], 0.5),
            new(typeIds["dark"], typeIds["ghost"], 2.0),
            new(typeIds["dark"], typeIds["steel"], 1.0),
            new(typeIds["dark"], typeIds["fairy"], 0.5),
            new(typeIds["dark"], typeIds["electric"], 1.0),
            new(typeIds["dark"], typeIds["poison"], 1.0),
            new(typeIds["dark"], typeIds["bug"], 1.0),
            new(typeIds["dark"], typeIds["fighting"], 0.5),
            new(typeIds["dark"], typeIds["ice"], 1.0),
            new(typeIds["dark"], typeIds["ground"], 1.0),
            new(typeIds["dark"], typeIds["rock"], 1.0),
            new(typeIds["dark"], typeIds["flying"], 1.0),
            new(typeIds["dark"], typeIds["dragon"], 1.0),

            // GHOST
            new(typeIds["ghost"], typeIds["normal"], 0.0),
            new(typeIds["ghost"], typeIds["water"], 1.0),
            new(typeIds["ghost"], typeIds["fire"], 1.0),
            new(typeIds["ghost"], typeIds["grass"], 1.0),
            new(typeIds["ghost"], typeIds["psychic"], 2.0),
            new(typeIds["ghost"], typeIds["dark"], 0.5),
            new(typeIds["ghost"], typeIds["ghost"], 2.0),
            new(typeIds["ghost"], typeIds["steel"], 1.0),
            new(typeIds["ghost"], typeIds["fairy"], 1.0),
            new(typeIds["ghost"], typeIds["electric"], 1.0),
            new(typeIds["ghost"], typeIds["poison"], 1.0),
            new(typeIds["ghost"], typeIds["bug"], 1.0),
            new(typeIds["ghost"], typeIds["fighting"], 1.0),
            new(typeIds["ghost"], typeIds["ice"], 1.0),
            new(typeIds["ghost"], typeIds["ground"], 1.0),
            new(typeIds["ghost"], typeIds["rock"], 1.0),
            new(typeIds["ghost"], typeIds["flying"], 1.0),
            new(typeIds["ghost"], typeIds["dragon"], 1.0),

            // STEEL
            new(typeIds["steel"], typeIds["normal"], 1.0),
            new(typeIds["steel"], typeIds["water"], 0.5),
            new(typeIds["steel"], typeIds["fire"], 0.5),
            new(typeIds["steel"], typeIds["grass"], 1.0),
            new(typeIds["steel"], typeIds["psychic"], 1.0),
            new(typeIds["steel"], typeIds["dark"], 1.0),
            new(typeIds["steel"], typeIds["ghost"], 1.0),
            new(typeIds["steel"], typeIds["steel"], 0.5),
            new(typeIds["steel"], typeIds["fairy"], 2.0),
            new(typeIds["steel"], typeIds["electric"], 0.5),
            new(typeIds["steel"], typeIds["poison"], 1.0),
            new(typeIds["steel"], typeIds["bug"], 1.0),
            new(typeIds["steel"], typeIds["fighting"], 1.0),
            new(typeIds["steel"], typeIds["ice"], 2.0),
            new(typeIds["steel"], typeIds["ground"], 1.0),
            new(typeIds["steel"], typeIds["rock"], 2.0),
            new(typeIds["steel"], typeIds["flying"], 1.0),
            new(typeIds["steel"], typeIds["dragon"], 1.0),

            // FAIRY
            new(typeIds["fairy"], typeIds["normal"], 1.0),
            new(typeIds["fairy"], typeIds["water"], 1.0),
            new(typeIds["fairy"], typeIds["fire"], 0.5),
            new(typeIds["fairy"], typeIds["grass"], 1.0),
            new(typeIds["fairy"], typeIds["psychic"], 1.0),
            new(typeIds["fairy"], typeIds["dark"], 2.0),
            new(typeIds["fairy"], typeIds["ghost"], 1.0),
            new(typeIds["fairy"], typeIds["steel"], 0.5),
            new(typeIds["fairy"], typeIds["fairy"], 1.0),
            new(typeIds["fairy"], typeIds["electric"], 1.0),
            new(typeIds["fairy"], typeIds["poison"], 0.5),
            new(typeIds["fairy"], typeIds["bug"], 1.0),
            new(typeIds["fairy"], typeIds["fighting"], 2.0),
            new(typeIds["fairy"], typeIds["ice"], 1.0),
            new(typeIds["fairy"], typeIds["ground"], 1.0),
            new(typeIds["fairy"], typeIds["rock"], 1.0),
            new(typeIds["fairy"], typeIds["flying"], 1.0),
            new(typeIds["fairy"], typeIds["dragon"], 2.0),

            // ELECTRIC
            new(typeIds["electric"], typeIds["normal"], 1.0),
            new(typeIds["electric"], typeIds["water"], 2.0),
            new(typeIds["electric"], typeIds["fire"], 1.0),
            new(typeIds["electric"], typeIds["grass"], 0.5),
            new(typeIds["electric"], typeIds["psychic"], 1.0),
            new(typeIds["electric"], typeIds["dark"], 1.0),
            new(typeIds["electric"], typeIds["ghost"], 1.0),
            new(typeIds["electric"], typeIds["steel"], 1.0),
            new(typeIds["electric"], typeIds["fairy"], 1.0),
            new(typeIds["electric"], typeIds["electric"], 0.5),
            new(typeIds["electric"], typeIds["poison"], 1.0),
            new(typeIds["electric"], typeIds["bug"], 1.0),
            new(typeIds["electric"], typeIds["fighting"], 1.0),
            new(typeIds["electric"], typeIds["ice"], 1.0),
            new(typeIds["electric"], typeIds["ground"], 0.0),
            new(typeIds["electric"], typeIds["rock"], 1.0),
            new(typeIds["electric"], typeIds["flying"], 1.0),
            new(typeIds["electric"], typeIds["dragon"], 0.5),

            // POISON
            new(typeIds["poison"], typeIds["normal"], 1.0),
            new(typeIds["poison"], typeIds["water"], 1.0),
            new(typeIds["poison"], typeIds["fire"], 1.0),
            new(typeIds["poison"], typeIds["grass"], 2.0),
            new(typeIds["poison"], typeIds["psychic"], 1.0),
            new(typeIds["poison"], typeIds["dark"], 1.0),
            new(typeIds["poison"], typeIds["ghost"], 0.5),
            new(typeIds["poison"], typeIds["steel"], 0.0),
            new(typeIds["poison"], typeIds["fairy"], 2.0),
            new(typeIds["poison"], typeIds["electric"], 1.0),
            new(typeIds["poison"], typeIds["poison"], 0.5),
            new(typeIds["poison"], typeIds["bug"], 1.0),
            new(typeIds["poison"], typeIds["fighting"], 1.0),
            new(typeIds["poison"], typeIds["ice"], 1.0),
            new(typeIds["poison"], typeIds["ground"], 0.5),
            new(typeIds["poison"], typeIds["rock"], 0.5),
            new(typeIds["poison"], typeIds["flying"], 1.0),
            new(typeIds["poison"], typeIds["dragon"], 1.0),

            // BUG
            new(typeIds["bug"], typeIds["normal"], 1.0),
            new(typeIds["bug"], typeIds["water"], 1.0),
            new(typeIds["bug"], typeIds["fire"], 0.5),
            new(typeIds["bug"], typeIds["grass"], 2.0),
            new(typeIds["bug"], typeIds["psychic"], 2.0),
            new(typeIds["bug"], typeIds["dark"], 2.0),
            new(typeIds["bug"], typeIds["ghost"], 0.5),
            new(typeIds["bug"], typeIds["steel"], 0.5),
            new(typeIds["bug"], typeIds["fairy"], 0.5),
            new(typeIds["bug"], typeIds["electric"], 1.0),
            new(typeIds["bug"], typeIds["poison"], 0.5),
            new(typeIds["bug"], typeIds["bug"], 1.0),
            new(typeIds["bug"], typeIds["fighting"], 0.5),
            new(typeIds["bug"], typeIds["ice"], 1.0),
            new(typeIds["bug"], typeIds["ground"], 1.0),
            new(typeIds["bug"], typeIds["rock"], 1.0),
            new(typeIds["bug"], typeIds["flying"], 0.5),
            new(typeIds["bug"], typeIds["dragon"], 1.0),

            // FIGHTING
            new(typeIds["fighting"], typeIds["normal"], 2.0),
            new(typeIds["fighting"], typeIds["water"], 1.0),
            new(typeIds["fighting"], typeIds["fire"], 1.0),
            new(typeIds["fighting"], typeIds["grass"], 1.0),
            new(typeIds["fighting"], typeIds["psychic"], 0.5),
            new(typeIds["fighting"], typeIds["dark"], 2.0),
            new(typeIds["fighting"], typeIds["ghost"], 0.0),
            new(typeIds["fighting"], typeIds["steel"], 2.0),
            new(typeIds["fighting"], typeIds["fairy"], 0.5),
            new(typeIds["fighting"], typeIds["electric"], 1.0),
            new(typeIds["fighting"], typeIds["poison"], 0.5),
            new(typeIds["fighting"], typeIds["bug"], 0.5),
            new(typeIds["fighting"], typeIds["fighting"], 1.0),
            new(typeIds["fighting"], typeIds["ice"], 2.0),
            new(typeIds["fighting"], typeIds["ground"], 1.0),
            new(typeIds["fighting"], typeIds["rock"], 2.0),
            new(typeIds["fighting"], typeIds["flying"], 0.5),
            new(typeIds["fighting"], typeIds["dragon"], 1.0),

            // ICE
            new(typeIds["ice"], typeIds["normal"], 1.0),
            new(typeIds["ice"], typeIds["water"], 0.5),
            new(typeIds["ice"], typeIds["fire"], 0.5),
            new(typeIds["ice"], typeIds["grass"], 2.0),
            new(typeIds["ice"], typeIds["psychic"], 1.0),
            new(typeIds["ice"], typeIds["dark"], 1.0),
            new(typeIds["ice"], typeIds["ghost"], 1.0),
            new(typeIds["ice"], typeIds["steel"], 0.5),
            new(typeIds["ice"], typeIds["fairy"], 1.0),
            new(typeIds["ice"], typeIds["electric"], 1.0),
            new(typeIds["ice"], typeIds["poison"], 1.0),
            new(typeIds["ice"], typeIds["bug"], 1.0),
            new(typeIds["ice"], typeIds["fighting"], 1.0),
            new(typeIds["ice"], typeIds["ice"], 1.0),
            new(typeIds["ice"], typeIds["ground"], 2.0),
            new(typeIds["ice"], typeIds["rock"], 1.0),
            new(typeIds["ice"], typeIds["flying"], 2.0),
            new(typeIds["ice"], typeIds["dragon"], 2.0),

            // GROUND
            new(typeIds["ground"], typeIds["normal"], 1.0),
            new(typeIds["ground"], typeIds["water"], 1.0),
            new(typeIds["ground"], typeIds["fire"], 2.0),
            new(typeIds["ground"], typeIds["grass"], 0.5),
            new(typeIds["ground"], typeIds["psychic"], 1.0),
            new(typeIds["ground"], typeIds["dark"], 1.0),
            new(typeIds["ground"], typeIds["ghost"], 1.0),
            new(typeIds["ground"], typeIds["steel"], 2.0),
            new(typeIds["ground"], typeIds["fairy"], 1.0),
            new(typeIds["ground"], typeIds["electric"], 2.0),
            new(typeIds["ground"], typeIds["poison"], 2.0),
            new(typeIds["ground"], typeIds["bug"], 0.5),
            new(typeIds["ground"], typeIds["fighting"], 1.0),
            new(typeIds["ground"], typeIds["ice"], 1.0),
            new(typeIds["ground"], typeIds["ground"], 1.0),
            new(typeIds["ground"], typeIds["rock"], 2.0),
            new(typeIds["ground"], typeIds["flying"], 0.0),
            new(typeIds["ground"], typeIds["dragon"], 1.0),

            // ROCK
            new(typeIds["rock"], typeIds["normal"], 1.0),
            new(typeIds["rock"], typeIds["water"], 1.0),
            new(typeIds["rock"], typeIds["fire"], 2.0),
            new(typeIds["rock"], typeIds["grass"], 1.0),
            new(typeIds["rock"], typeIds["psychic"], 1.0),
            new(typeIds["rock"], typeIds["dark"], 1.0),
            new(typeIds["rock"], typeIds["ghost"], 1.0),
            new(typeIds["rock"], typeIds["steel"], 0.5),
            new(typeIds["rock"], typeIds["fairy"], 1.0),
            new(typeIds["rock"], typeIds["electric"], 1.0),
            new(typeIds["rock"], typeIds["poison"], 1.0),
            new(typeIds["rock"], typeIds["bug"], 2.0),
            new(typeIds["rock"], typeIds["fighting"], 0.5),
            new(typeIds["rock"], typeIds["ice"], 2.0),
            new(typeIds["rock"], typeIds["ground"], 0.5),
            new(typeIds["rock"], typeIds["rock"], 1.0),
            new(typeIds["rock"], typeIds["flying"], 2.0),
            new(typeIds["rock"], typeIds["dragon"], 1.0),

            // FLYING
            new(typeIds["flying"], typeIds["normal"], 1.0),
            new(typeIds["flying"], typeIds["water"], 1.0),
            new(typeIds["flying"], typeIds["fire"], 1.0),
            new(typeIds["flying"], typeIds["grass"], 2.0),
            new(typeIds["flying"], typeIds["psychic"], 1.0),
            new(typeIds["flying"], typeIds["dark"], 1.0),
            new(typeIds["flying"], typeIds["ghost"], 1.0),
            new(typeIds["flying"], typeIds["steel"], 0.5),
            new(typeIds["flying"], typeIds["fairy"], 1.0),
            new(typeIds["flying"], typeIds["electric"], 0.5),
            new(typeIds["flying"], typeIds["poison"], 1.0),
            new(typeIds["flying"], typeIds["bug"], 2.0),
            new(typeIds["flying"], typeIds["fighting"], 2.0),
            new(typeIds["flying"], typeIds["ice"], 1.0),
            new(typeIds["flying"], typeIds["ground"], 1.0),
            new(typeIds["flying"], typeIds["rock"], 0.5),
            new(typeIds["flying"], typeIds["flying"], 1.0),
            new(typeIds["flying"], typeIds["dragon"], 1.0),

            // DRAGON
            new(typeIds["dragon"], typeIds["normal"], 1.0),
            new(typeIds["dragon"], typeIds["water"], 1.0),
            new(typeIds["dragon"], typeIds["fire"], 1.0),
            new(typeIds["dragon"], typeIds["grass"], 1.0),
            new(typeIds["dragon"], typeIds["psychic"], 1.0),
            new(typeIds["dragon"], typeIds["dark"], 1.0),
            new(typeIds["dragon"], typeIds["ghost"], 1.0),
            new(typeIds["dragon"], typeIds["steel"], 0.5),
            new(typeIds["dragon"], typeIds["fairy"], 0.0),
            new(typeIds["dragon"], typeIds["electric"], 1.0),
            new(typeIds["dragon"], typeIds["poison"], 1.0),
            new(typeIds["dragon"], typeIds["bug"], 1.0),
            new(typeIds["dragon"], typeIds["fighting"], 1.0),
            new(typeIds["dragon"], typeIds["ice"], 1.0),
            new(typeIds["dragon"], typeIds["ground"], 1.0),
            new(typeIds["dragon"], typeIds["rock"], 1.0),
            new(typeIds["dragon"], typeIds["flying"], 1.0),
            new(typeIds["dragon"], typeIds["dragon"], 2.0)
        };
        
        context.TypeEffectivenesses.AddRange(allEffectivenesses);
        context.SaveChanges();
    }
}