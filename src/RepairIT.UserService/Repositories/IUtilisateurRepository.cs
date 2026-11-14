using RepairIT.Shared.Models;

namespace RepairIT.UserService.Repositories;

/// <summary>Accès aux données des utilisateurs (isolé du métier pour pouvoir le simuler dans les tests).</summary>
public interface IUtilisateurRepository
{
    Task<List<Utilisateur>> ListerAsync();

    Task<Utilisateur?> TrouverParIdAsync(int id);

    Task<bool> EmailExisteAsync(string email);

    Task AjouterAsync(Utilisateur utilisateur);
}
