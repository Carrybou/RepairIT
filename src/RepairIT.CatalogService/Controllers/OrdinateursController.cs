using Microsoft.AspNetCore.Mvc;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Controllers;

/// <summary>CRUD des ordinateurs du catalogue. La logique métier est déléguée à <see cref="IOrdinateurService"/>.</summary>
[ApiController]
[Route("api/ordinateurs")]
public class OrdinateursController(IOrdinateurService ordinateurService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Ordinateur>>> Lister() =>
        Ok(await ordinateurService.ListerAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ordinateur>> ObtenirParId(int id)
    {
        var ordinateur = await ordinateurService.ObtenirParIdAsync(id);
        return ordinateur is null ? NotFound() : Ok(ordinateur);
    }

    [HttpPost]
    public async Task<ActionResult<Ordinateur>> Ajouter(Ordinateur ordinateur)
    {
        var ordinateurAjoute = await ordinateurService.AjouterAsync(ordinateur);
        return CreatedAtAction(nameof(ObtenirParId), new { id = ordinateurAjoute.Id }, ordinateurAjoute);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Ordinateur>> Modifier(int id, Ordinateur ordinateur)
    {
        var ordinateurModifie = await ordinateurService.ModifierAsync(id, ordinateur);
        return ordinateurModifie is null ? NotFound() : Ok(ordinateurModifie);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Supprimer(int id) =>
        await ordinateurService.SupprimerAsync(id) ? NoContent() : NotFound();
}
