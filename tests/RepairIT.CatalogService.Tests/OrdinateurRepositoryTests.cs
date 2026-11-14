using RepairIT.CatalogService.Repositories;

namespace RepairIT.CatalogService.Tests;

public class OrdinateurRepositoryTests : BaseDeTestSqlite
{
    private readonly OrdinateurRepository repository;

    public OrdinateurRepositoryTests()
    {
        repository = new OrdinateurRepository(Contexte);
    }

    [Fact]
    public async Task AjouterAsync_AttribueUnIdEtPersisteLOrdinateur()
    {
        var ordinateur = OrdinateurFabrique.Creer();

        await repository.AjouterAsync(ordinateur);

        Assert.True(ordinateur.Id > 0);
        Assert.NotNull(await repository.TrouverParIdAsync(ordinateur.Id));
    }

    [Fact]
    public async Task ListerAsync_RetourneToutTrieParMarquePuisNom()
    {
        await repository.AjouterAsync(OrdinateurFabrique.Creer(nom: "Zenbook", marque: "ASUS"));
        await repository.AjouterAsync(OrdinateurFabrique.Creer(nom: "XPS", marque: "Dell"));
        await repository.AjouterAsync(OrdinateurFabrique.Creer(nom: "ROG", marque: "ASUS"));

        var resultats = await repository.ListerAsync();

        Assert.Equal(["ROG", "Zenbook", "XPS"], resultats.Select(o => o.Nom));
    }

    [Fact]
    public async Task SupprimerAsync_RetireLOrdinateurDeLaBase()
    {
        var ordinateur = OrdinateurFabrique.Creer();
        await repository.AjouterAsync(ordinateur);

        await repository.SupprimerAsync(ordinateur);

        Assert.Null(await repository.TrouverParIdAsync(ordinateur.Id));
    }

    [Fact]
    public async Task SauvegarderAsync_PersisteLesModificationsDUneEntiteSuivie()
    {
        var ordinateur = OrdinateurFabrique.Creer(stock: 1);
        await repository.AjouterAsync(ordinateur);

        ordinateur.Stock = 9;
        await repository.SauvegarderAsync();
        Contexte.ChangeTracker.Clear();

        Assert.Equal(9, (await repository.TrouverParIdAsync(ordinateur.Id))!.Stock);
    }
}
