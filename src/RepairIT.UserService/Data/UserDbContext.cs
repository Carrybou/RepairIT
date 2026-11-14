using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;

namespace RepairIT.UserService.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var utilisateur = modelBuilder.Entity<Utilisateur>();

        utilisateur.ToTable("Utilisateurs");
        utilisateur.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        utilisateur.HasIndex(u => u.Email).IsUnique();
    }
}
