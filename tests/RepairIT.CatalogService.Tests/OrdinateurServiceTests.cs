using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.CatalogService.Repositories;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Tests;

public class OrdinateurServiceTests
{
    private static readonly DateTimeOffset Maintenant = new(2026, 11, 14, 10, 0, 0, TimeSpan.Zero);

    private readonly Mock<IOrdinateurRepository> repository = new();
    private readonly OrdinateurService service;

    public OrdinateurServiceTests()
    {
        var horloge = new Mock<TimeProvider>();
        horloge.Setup(h => h.GetUtcNow()).Returns(Maintenant);

        service = new OrdinateurService(
            repository.Object,
            horloge.Object,
            NullLogger<OrdinateurService>.Instance);
    }

    [Fact]
    public async Task ListerAsync_RetourneLesOrdinateursDuRepository()
    {
        repository.Setup(r => r.ListerAsync()).ReturnsAsync([OrdinateurFabrique.Creer(marque: "ASUS")]);

        var resultats = await service.ListerAsync();

        Assert.Single(resultats);
    }

    [Fact]
    public async Task AjouterAsync_RenseigneLaDateDAjoutEtIgnoreLIdFourni()
    {
        var ordinateur = OrdinateurFabrique.Creer(id: 42);

        var resultat = await service.AjouterAsync(ordinateur);

        Assert.Equal(0, resultat.Id);
        Assert.Equal(Maintenant.UtcDateTime, resultat.DateAjout);
        repository.Verify(r => r.AjouterAsync(ordinateur), Times.Once);
    }

    [Fact]
    public async Task ModifierAsync_OrdinateurExistant_AppliqueLesModificationsEtSauvegarde()
    {
        var existant = OrdinateurFabrique.Creer(nom: "Ancien nom", id: 1);
        repository.Setup(r => r.TrouverParIdAsync(1)).ReturnsAsync(existant);

        var resultat = await service.ModifierAsync(1, OrdinateurFabrique.Creer(nom: "Nouveau nom", prix: 750m));

        Assert.NotNull(resultat);
        Assert.Equal("Nouveau nom", resultat.Nom);
        Assert.Equal(750m, resultat.Prix);
        repository.Verify(r => r.SauvegarderAsync(), Times.Once);
    }

    [Fact]
    public async Task ModifierAsync_OrdinateurInexistant_RetourneNullSansSauvegarder()
    {
        repository.Setup(r => r.TrouverParIdAsync(It.IsAny<int>())).ReturnsAsync((Ordinateur?)null);

        var resultat = await service.ModifierAsync(404, OrdinateurFabrique.Creer());

        Assert.Null(resultat);
        repository.Verify(r => r.SauvegarderAsync(), Times.Never);
    }

    [Fact]
    public async Task SupprimerAsync_OrdinateurExistant_LeSupprime()
    {
        var existant = OrdinateurFabrique.Creer(id: 1);
        repository.Setup(r => r.TrouverParIdAsync(1)).ReturnsAsync(existant);

        var supprime = await service.SupprimerAsync(1);

        Assert.True(supprime);
        repository.Verify(r => r.SupprimerAsync(existant), Times.Once);
    }

    [Fact]
    public async Task SupprimerAsync_OrdinateurInexistant_RetourneFalse()
    {
        var supprime = await service.SupprimerAsync(404);

        Assert.False(supprime);
        repository.Verify(r => r.SupprimerAsync(It.IsAny<Ordinateur>()), Times.Never);
    }
}
