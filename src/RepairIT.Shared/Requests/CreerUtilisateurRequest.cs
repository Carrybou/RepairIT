using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Requests;

public record CreerUtilisateurRequest(
    [Required, StringLength(50)] string Nom,
    [Required, StringLength(50)] string Prenom,
    [Required, EmailAddress, StringLength(150)] string Email,
    RoleUtilisateur Role);
