using Microsoft.AspNetCore.Mvc;
using Moq;
using RepairIT.CatalogService.Controllers;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Tests;

public class OrdinateursControllerTests
{
    private readonly Mock<IOrdinateurService> service = new();
    private readonly OrdinateursController controller;

    public OrdinateursControllerTests()
    {
        controller = new OrdinateursController(service.Object);
    }

    [Fact]
    public async Task Lister_RetourneOkAvecLesOrdinateurs()
    {
        service.Setup(s => s.ListerAsync()).ReturnsAsync([OrdinateurFabrique.Creer()]);

        var reponse = await controller.Lister();

        var ok = Assert.IsType<OkObjectResult>(reponse.Result);
        Assert.Single(Assert.IsAssignableFrom<IReadOnlyList<Ordinateur>>(ok.Value));
    }

    [Fact]
    public async Task ObtenirParId_OrdinateurInexistant_RetourneNotFound()
    {
        var reponse = await controller.ObtenirParId(404);

        Assert.IsType<NotFoundResult>(reponse.Result);
    }

    [Fact]
    public async Task ObtenirParId_OrdinateurExistant_RetourneOk()
    {
        service.Setup(s => s.ObtenirParIdAsync(1)).ReturnsAsync(OrdinateurFabrique.Creer(id: 1));

        var reponse = await controller.ObtenirParId(1);

        Assert.IsType<OkObjectResult>(reponse.Result);
    }

    [Fact]
    public async Task Ajouter_RetourneCreatedAvecLEmplacementDuNouvelOrdinateur()
    {
        var ordinateur = OrdinateurFabrique.Creer();
        service.Setup(s => s.AjouterAsync(ordinateur)).ReturnsAsync(OrdinateurFabrique.Creer(id: 8));

        var reponse = await controller.Ajouter(ordinateur);

        var created = Assert.IsType<CreatedAtActionResult>(reponse.Result);
        Assert.Equal(8, created.RouteValues!["id"]);
    }

    [Fact]
    public async Task Modifier_OrdinateurInexistant_RetourneNotFound()
    {
        var reponse = await controller.Modifier(404, OrdinateurFabrique.Creer());

        Assert.IsType<NotFoundResult>(reponse.Result);
    }

    [Theory]
    [InlineData(true, typeof(NoContentResult))]
    [InlineData(false, typeof(NotFoundResult))]
    public async Task Supprimer_RetourneLeCodeAdapte(bool supprime, Type typeAttendu)
    {
        service.Setup(s => s.SupprimerAsync(1)).ReturnsAsync(supprime);

        var reponse = await controller.Supprimer(1);

        Assert.IsType(typeAttendu, reponse);
    }
}
