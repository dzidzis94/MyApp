using Microsoft.EntityFrameworkCore;
using MyApp.Web.Models.Entities;

namespace MyApp.Web.Data
{
    public class DarbuContext : DbContext
    {
        public DarbuContext(DbContextOptions<DarbuContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectSubSection> ProjectSubSections { get; set; } = null!;
        public DbSet<ProjectSubSectionSubmission> ProjectSubSectionSubmissions { get; set; } = null!;
        public DbSet<RecentActivity> RecentActivities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> Admin (1:1)
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId);

            // User -> Client (1:1)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Client>(c => c.UserId);

            // Admins -> Projects (1:N)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById);

            // Projects -> ProjectSubSections (1:N)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.SubSections)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProjectSubSections -> ProjectSubSectionSubmissions (1:N)
            modelBuilder.Entity<ProjectSubSectionSubmission>()
                .HasOne(s => s.ProjectSubSection)
                .WithMany(ss => ss.Submissions)
                .HasForeignKey(s => s.SubSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Clients -> ProjectSubSectionSubmissions (1:N)
            modelBuilder.Entity<ProjectSubSectionSubmission>()
                .HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey(s => s.ClientId);

            // Users -> RecentActivity (1:N)
            modelBuilder.Entity<RecentActivity>()
                .HasOne(ra => ra.User)
                .WithMany()
                .HasForeignKey(ra => ra.UserId);
        }
    }
}