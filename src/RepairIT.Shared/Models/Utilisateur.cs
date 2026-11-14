using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Models;

/// <summary>Utilisateur de l'application. Aucun mot de passe n'est stocké : l'authentification sera déléguée à Keycloak.</summary>
public class Utilisateur
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string UserKeycloak { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Nom { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Prenom { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    public RoleUtilisateur Role { get; set; }

    public DateTime DateCreation { get; set; }
}
