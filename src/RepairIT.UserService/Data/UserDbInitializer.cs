using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;
using RepairIT.UserService.Options;

namespace RepairIT.UserService.Data;

/// <summary>Crée la base si besoin et y insère 1 administrateur et 4 gestionnaires lorsqu'elle est vide.</summary>
public class UserDbInitializer(
    UserDbContext context,
    IOptions<UtilisateurOptions> options,
    ILogger<UserDbInitializer> logger)
{
    public async Task InitialiserAsync()
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Utilisateurs.AnyAsync())
        {
            logger.LogInformation("Utilisateurs déjà initialisés, aucune donnée ajoutée");
            return;
        }

        var utilisateurs = CreerUtilisateursDeDemonstration();
        context.Utilisateurs.AddRange(utilisateurs);
        await context.SaveChangesAsync();

        logger.LogInformation("Base utilisateurs initialisée avec {Nombre} comptes", utilisateurs.Count);
    }

    private List<Utilisateur> CreerUtilisateursDeDemonstration() =>
    [
        Creer("Martin", "Alice", "alice.martin@repairit.fr", RoleUtilisateur.Administrateur),
        Creer("Bernard", "Hugo", "hugo.bernard@repairit.fr", RoleUtilisateur.Gestionnaire),
        Creer("Petit", "Chloé", "chloe.petit@repairit.fr", RoleUtilisateur.Gestionnaire),
        Creer("Durand", "Lucas", "lucas.durand@repairit.fr", RoleUtilisateur.Gestionnaire),
        Creer("Moreau", "Emma", "emma.moreau@repairit.fr", RoleUtilisateur.Gestionnaire)
    ];

    private Utilisateur Creer(string nom, string prenom, string email, RoleUtilisateur role) => new()
    {
        Nom = nom,
        Prenom = prenom,
        Email = email,
        Role = role,
        UserKeycloak = options.Value.UserKeycloakParDefaut,
        DateCreation = DateTime.UtcNow
    };
}
