using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Repositories;

/// <summary>Accès aux données des ordinateurs (isolé du métier pour pouvoir le simuler dans les tests).</summary>
public interface IOrdinateurRepository
{
    Task<List<Ordinateur>> ListerAsync();

    Task<Ordinateur?> TrouverParIdAsync(int id);

    Task AjouterAsync(Ordinateur ordinateur);

    Task SupprimerAsync(Ordinateur ordinateur);

    Task SauvegarderAsync();
}
