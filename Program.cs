using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;
using NetDexQL.Data;
using NetDexQL.GraphQL.Queries;
using NetDexQL.Data.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
{
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy()
    }
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ApplicationDbContext>();

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<IMonTypeRepository, MonTypeRepository>();

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>()
    .AddQueryType<QueryType>()
    .AddProjections()
    .AddFiltering()
    .AddType<PokemonFilterInputType>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    await SeedDb.Initialize(services);
}

app.UseRouting();
app.MapGraphQL();
app.MapBananaCakePop();

app.Run();