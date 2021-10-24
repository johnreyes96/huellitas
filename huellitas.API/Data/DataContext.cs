using huellitas.API.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace huellitas.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Billing> Billings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DocumentType>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<PetType>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<Service>().HasIndex(x => x.Description).IsUnique();
            modelBuilder.Entity<Pet>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Billing>().HasIndex(x => x.Id).IsUnique();
        }
    }
}
