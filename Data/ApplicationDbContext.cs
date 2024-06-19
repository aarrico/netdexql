using Microsoft.EntityFrameworkCore;
using NetDexQL.Data.Models;

namespace NetDexQL.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Pokemon> Pokemon { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pokemon>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();
        }
    }
}