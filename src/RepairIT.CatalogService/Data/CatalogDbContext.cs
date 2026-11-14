using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Ordinateur> Ordinateurs => Set<Ordinateur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ordinateur = modelBuilder.Entity<Ordinateur>();

        ordinateur.ToTable("Ordinateurs");
        ordinateur.Property(o => o.Type).HasConversion<string>().HasMaxLength(20);
        ordinateur.Property(o => o.Prix).HasPrecision(10, 2);
        ordinateur.HasIndex(o => new { o.Marque, o.Nom });
    }
}
