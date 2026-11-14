using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Tests;

/// <summary>Construit des ordinateurs valides pour les tests, en ne précisant que les champs utiles au test.</summary>
internal static class OrdinateurFabrique
{
    public static Ordinateur Creer(string nom = "PC Test", string marque = "Dell", int stock = 5, decimal prix = 1000m, int id = 0) => new()
    {
        Id = id,
        Nom = nom,
        Marque = marque,
        Processeur = "Intel Core i5",
        RamGo = 16,
        StockageGo = 512,
        Type = TypeOrdinateur.Portable,
        Prix = prix,
        Stock = stock
    };
}
