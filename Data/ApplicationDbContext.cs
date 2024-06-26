using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;

namespace NetDexQL.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Pokemon> Pokemon { get; set; }
    public DbSet<MonType> Types { get; set; }
    public DbSet<PokemonOnType> PokemonOnTypes { get; set; }
    public DbSet<TypeEffectiveness> TypeEffectivenesses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PokemonConfiguration());
        modelBuilder.ApplyConfiguration(new TypeConfiguration());
        modelBuilder.ApplyConfiguration(new TypeEffectivenessConfiguration());
        
         modelBuilder.Entity<MonType>()
            .HasMany(t => t.Pokemon)
            .WithMany(t => t.MonTypes)
            .UsingEntity<PokemonOnType>();


        modelBuilder.Entity<TypeEffectiveness>()
            .HasOne(te => te.AttackingType)
            .WithMany(t => t.AttackingTypeEffectivenesses)
            .HasForeignKey(te => te.AttackingTypeId);

        modelBuilder.Entity<TypeEffectiveness>()
            .HasOne(te => te.DefendingType)
            .WithMany(t => t.DefendingTypeEffectivenesses)
            .HasForeignKey(te => te.DefendingTypeId);
    }
}