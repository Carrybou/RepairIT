using RepairIT.CatalogService.Repositories;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Services;

public class OrdinateurService(
    IOrdinateurRepository repository,
    TimeProvider horloge,
    ILogger<OrdinateurService> logger) : IOrdinateurService
{
    public async Task<IReadOnlyList<Ordinateur>> ListerAsync() => await repository.ListerAsync();

    public Task<Ordinateur?> ObtenirParIdAsync(int id) => repository.TrouverParIdAsync(id);

    public async Task<Ordinateur> AjouterAsync(Ordinateur ordinateur)
    {
        // L'identifiant est attribué par la base et la date d'ajout par le serveur, jamais par le client.
        ordinateur.Id = 0;
        ordinateur.DateAjout = horloge.GetUtcNow().UtcDateTime;
        await repository.AjouterAsync(ordinateur);

        logger.LogInformation("Ordinateur {Id} ajouté : {Marque} {Nom}", ordinateur.Id, ordinateur.Marque, ordinateur.Nom);
        return ordinateur;
    }

    public async Task<Ordinateur?> ModifierAsync(int id, Ordinateur modifications)
    {
        var ordinateur = await TrouverOuJournaliserAsync(id);
        if (ordinateur is null) return null;

        ordinateur.MettreAJourDepuis(modifications);
        await repository.SauvegarderAsync();

        logger.LogInformation("Ordinateur {Id} modifié", id);
        return ordinateur;
    }

    public async Task<bool> SupprimerAsync(int id)
    {
        var ordinateur = await TrouverOuJournaliserAsync(id);
        if (ordinateur is null) return false;

        await repository.SupprimerAsync(ordinateur);

        logger.LogInformation("Ordinateur {Id} supprimé", id);
        return true;
    }

    private async Task<Ordinateur?> TrouverOuJournaliserAsync(int id)
    {
        var ordinateur = await repository.TrouverParIdAsync(id);
        if (ordinateur is null)
        {
            logger.LogWarning("Ordinateur {Id} introuvable", id);
        }

        return ordinateur;
    }
}
