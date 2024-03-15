using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Password> Passwords { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
           .HasMany(u => u.Passwords) // Un utilisateur peut avoir plusieurs mots de passe
           .WithOne(p => p.User) // Chaque mot de passe appartient à un seul utilisateur
           .HasForeignKey(p => p.UserId);
        }
    }
}
