using Microsoft.EntityFrameworkCore;
using Timelapse.CLI.Entities;

namespace Timelapse.CLI.Infraestructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Period> Periods { get; set; }

        public string DbPath { get; }

        public ApplicationDbContext()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directory = Path.Combine(appDataPath, "Timelapse");

            DbPath = Path.Combine(directory, "tml.db");

            if (!File.Exists(DbPath))
            {
                Directory.CreateDirectory(directory);
                File.Create(DbPath).Dispose();
            }

            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"DataSource=file:{DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .ToTable("Items")
                .HasMany(c => c.Periods)
                .WithOne(c => c.Item)
                .HasForeignKey(c => c.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .ToTable("Items")
                .Property(w => w.Name)
                .IsRequired();

            modelBuilder.Entity<Item>()
                .Navigation(e => e.Periods)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
