using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;

namespace NetDexQL.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Pokemon> Pokemon { get; set; }
    public DbSet<MonType> MonTypes { get; set; }
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
        modelBuilder.ApplyConfiguration(new MonTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PokemonOnTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TypeEffectivenessConfiguration());

        modelBuilder.Entity<PokemonOnType>()
            .HasOne(pt => pt.Pokemon)
            .WithMany(p => p.PokemonTypes)
            .HasForeignKey(pt => pt.PokemonId);

        modelBuilder.Entity<PokemonOnType>()
            .HasOne(pt => pt.MonType)
            .WithMany(p => p.PokemonTypes)
            .HasForeignKey(pt => pt.MonTypeId);

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