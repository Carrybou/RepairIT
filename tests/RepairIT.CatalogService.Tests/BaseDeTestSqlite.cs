using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;

namespace RepairIT.CatalogService.Tests;

/// <summary>Fournit un contexte EF Core sur une base SQLite en mémoire, recréée pour chaque test.</summary>
public abstract class BaseDeTestSqlite : IDisposable
{
    private readonly SqliteConnection connexion = new("DataSource=:memory:");

    protected CatalogDbContext Contexte { get; }

    protected BaseDeTestSqlite()
    {
        connexion.Open();
        var options = new DbContextOptionsBuilder<CatalogDbContext>().UseSqlite(connexion).Options;
        Contexte = new CatalogDbContext(options);
        Contexte.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Contexte.Dispose();
        connexion.Dispose();
        GC.SuppressFinalize(this);
    }
}
