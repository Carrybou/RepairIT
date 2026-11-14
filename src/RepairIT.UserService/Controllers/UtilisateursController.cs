using Microsoft.AspNetCore.Mvc;
using RepairIT.Shared.Models;
using RepairIT.Shared.Requests;
using RepairIT.UserService.Services;

namespace RepairIT.UserService.Controllers;

/// <summary>Création et consultation des utilisateurs. La logique métier est déléguée à <see cref="IUtilisateurService"/>.</summary>
[ApiController]
[Route("api/utilisateurs")]
public class UtilisateursController(IUtilisateurService utilisateurService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Utilisateur>>> Lister() =>
        Ok(await utilisateurService.ListerAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Utilisateur>> ObtenirParId(int id)
    {
        var utilisateur = await utilisateurService.ObtenirParIdAsync(id);
        return utilisateur is null ? NotFound() : Ok(utilisateur);
    }

    [HttpPost]
    public async Task<ActionResult<Utilisateur>> Creer(CreerUtilisateurRequest requete)
    {
        // L'email est unique : on renvoie 409 plutôt que de laisser la base lever une exception.
        if (await utilisateurService.EmailDejaUtiliseAsync(requete.Email))
        {
            return Conflict($"Un utilisateur utilise déjà l'adresse {requete.Email}.");
        }

        var utilisateur = await utilisateurService.CreerAsync(requete);
        return CreatedAtAction(nameof(ObtenirParId), new { id = utilisateur.Id }, utilisateur);
    }
}
