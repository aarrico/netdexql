using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;
using NetDexQL.Data;
using NetDexQL.GraphQL.Queries;
using NetDexQL.Data.Repositories;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContextFactory<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<Pokemon>()
    .RegisterDbContext<ApplicationDbContext>();


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

app.Run();
