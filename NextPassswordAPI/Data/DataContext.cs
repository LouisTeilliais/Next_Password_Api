using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextPassswordAPI.Entities;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
           .HasMany(u => u.Passwords) 
           .WithOne(p => p.User) 
           .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Password>()
            .HasOne(p => p.Token) 
            .WithOne(t => t.Password) 
            .HasForeignKey<Token>(t => t.PasswordId);
        }
    }
}
