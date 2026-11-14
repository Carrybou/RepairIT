using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Services;

/// <summary>Règles métier du catalogue (CRUD des ordinateurs).</summary>
public interface IOrdinateurService
{
    Task<IReadOnlyList<Ordinateur>> ListerAsync();

    Task<Ordinateur?> ObtenirParIdAsync(int id);

    Task<Ordinateur> AjouterAsync(Ordinateur ordinateur);

    /// <summary>Retourne l'ordinateur modifié, ou null s'il n'existe pas.</summary>
    Task<Ordinateur?> ModifierAsync(int id, Ordinateur modifications);

    /// <summary>Retourne false si l'ordinateur n'existe pas.</summary>
    Task<bool> SupprimerAsync(int id);
}
