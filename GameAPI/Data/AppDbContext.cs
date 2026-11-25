using Microsoft.EntityFrameworkCore;
using GameApi.Models;

namespace GameApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Character> Characters => Set<Character>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .Property(a => a.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Account>()
                .Property(a => a.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<Character>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Character>()
                .Property(c => c.Class)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Character>()
                .HasOne(c => c.Account)
                .WithMany(a => a.Characters)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
