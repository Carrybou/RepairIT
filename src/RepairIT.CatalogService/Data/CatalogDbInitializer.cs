using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Data;

/// <summary>Crée la base si besoin et y insère un catalogue de démonstration lorsqu'elle est vide.</summary>
public class CatalogDbInitializer(CatalogDbContext context, ILogger<CatalogDbInitializer> logger)
{
    public async Task InitialiserAsync()
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Ordinateurs.AnyAsync())
        {
            logger.LogInformation("Catalogue déjà initialisé, aucune donnée ajoutée");
            return;
        }

        var ordinateurs = CreerCatalogueDeDemonstration();
        context.Ordinateurs.AddRange(ordinateurs);
        await context.SaveChangesAsync();

        logger.LogInformation("Catalogue initialisé avec {Nombre} ordinateurs", ordinateurs.Count);
    }

    private static List<Ordinateur> CreerCatalogueDeDemonstration()
    {
        var aujourdhui = DateTime.UtcNow;

        return
        [
            Creer("ROG Strix G16", "ASUS", "Intel Core i7-13650HX", 16, 1024, TypeOrdinateur.Portable, 1499m, 4, aujourdhui),
            Creer("MacBook Pro 14", "Apple", "Apple M3 Pro", 18, 512, TypeOrdinateur.Portable, 2399m, 2, aujourdhui),
            Creer("ThinkPad T14", "Lenovo", "AMD Ryzen 7 PRO 7840U", 32, 1024, TypeOrdinateur.Portable, 1290m, 6, aujourdhui),
            Creer("OptiPlex 7010", "Dell", "Intel Core i5-13500", 16, 512, TypeOrdinateur.Fixe, 899m, 8, aujourdhui),
            Creer("iMac 24", "Apple", "Apple M3", 8, 256, TypeOrdinateur.ToutEnUn, 1599m, 1, aujourdhui),
            Creer("ProLiant DL380", "HPE", "Intel Xeon Silver 4410Y", 64, 4096, TypeOrdinateur.Serveur, 4890m, 0, aujourdhui)
        ];
    }

    private static Ordinateur Creer(string nom, string marque, string processeur, int ramGo, int stockageGo,
        TypeOrdinateur type, decimal prix, int stock, DateTime dateAjout) => new()
    {
        Nom = nom,
        Marque = marque,
        Processeur = processeur,
        RamGo = ramGo,
        StockageGo = stockageGo,
        Type = type,
        Prix = prix,
        Stock = stock,
        DateAjout = dateAjout
    };
}
