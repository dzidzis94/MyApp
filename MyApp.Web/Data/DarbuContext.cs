using Microsoft.EntityFrameworkCore;
using MyApp.Web.Models;

namespace MyApp.Web.Data
{
    public class DarbuContext : DbContext
    {
        public DarbuContext(DbContextOptions<DarbuContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }

        // 👇 Pievieno šo metodi, lai konfigurētu Identity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurē Admin entītiju
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.Id); // Primārais atslēga
                entity.Property(a => a.Id)
                      .ValueGeneratedOnAdd(); // Identity īpašība
            });

            // Konfigurē Client entītiju
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                      .ValueGeneratedOnAdd();
            });

            // Konfigurē Project entītiju
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                      .ValueGeneratedOnAdd();
            });
        }
    }
}