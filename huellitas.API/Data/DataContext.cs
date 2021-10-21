using huellitas.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace huellitas.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<PetType> PetTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DocumentType>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<PetType>().HasIndex(x => x.Description).IsUnique();
        }
    }
}
