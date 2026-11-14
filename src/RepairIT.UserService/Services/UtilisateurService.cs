using Microsoft.Extensions.Options;
using RepairIT.Shared.Models;
using RepairIT.Shared.Requests;
using RepairIT.UserService.Options;
using RepairIT.UserService.Repositories;

namespace RepairIT.UserService.Services;

public class UtilisateurService(
    IUtilisateurRepository repository,
    IOptions<UtilisateurOptions> options,
    TimeProvider horloge,
    ILogger<UtilisateurService> logger) : IUtilisateurService
{
    public async Task<IReadOnlyList<Utilisateur>> ListerAsync() => await repository.ListerAsync();

    public Task<Utilisateur?> ObtenirParIdAsync(int id) => repository.TrouverParIdAsync(id);

    public Task<bool> EmailDejaUtiliseAsync(string email) => repository.EmailExisteAsync(NormaliserEmail(email));

    public async Task<Utilisateur> CreerAsync(CreerUtilisateurRequest requete)
    {
        var utilisateur = new Utilisateur
        {
            Nom = requete.Nom.Trim(),
            Prenom = requete.Prenom.Trim(),
            Email = NormaliserEmail(requete.Email),
            Role = requete.Role,
            // Pas de Keycloak en V1 : valeur par défaut lue dans appsettings.json.
            UserKeycloak = options.Value.UserKeycloakParDefaut,
            DateCreation = horloge.GetUtcNow().UtcDateTime
        };

        await repository.AjouterAsync(utilisateur);

        logger.LogInformation("Utilisateur {Id} créé avec le rôle {Role}", utilisateur.Id, utilisateur.Role);
        return utilisateur;
    }

    /// <summary>Emails stockés en minuscules et sans espaces pour que l'unicité ne dépende pas de la casse.</summary>
    private static string NormaliserEmail(string email) => email.Trim().ToLowerInvariant();
}
