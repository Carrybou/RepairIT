namespace RepairIT.UserService.Options;

/// <summary>Paramètres des utilisateurs, lus depuis la section « Utilisateurs » de appsettings.json.</summary>
public class UtilisateurOptions
{
    public const string Section = "Utilisateurs";

    /// <summary>Identifiant Keycloak provisoire : la liaison réelle avec Keycloak arrivera en V5.</summary>
    public string UserKeycloakParDefaut { get; set; } = "non-associe";
}
