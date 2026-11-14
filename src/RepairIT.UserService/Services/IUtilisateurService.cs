using RepairIT.Shared.Models;
using RepairIT.Shared.Requests;

namespace RepairIT.UserService.Services;

/// <summary>Règles métier des utilisateurs (création et consultation).</summary>
public interface IUtilisateurService
{
    Task<IReadOnlyList<Utilisateur>> ListerAsync();

    Task<Utilisateur?> ObtenirParIdAsync(int id);

    /// <summary>Indique si l'email (normalisé) est déjà associé à un utilisateur.</summary>
    Task<bool> EmailDejaUtiliseAsync(string email);

    Task<Utilisateur> CreerAsync(CreerUtilisateurRequest requete);
}
