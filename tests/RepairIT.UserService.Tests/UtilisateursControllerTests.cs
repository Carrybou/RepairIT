using Microsoft.AspNetCore.Mvc;
using Moq;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;
using RepairIT.Shared.Requests;
using RepairIT.UserService.Controllers;
using RepairIT.UserService.Services;

namespace RepairIT.UserService.Tests;

public class UtilisateursControllerTests
{
    private readonly Mock<IUtilisateurService> service = new();
    private readonly UtilisateursController controller;
    private readonly CreerUtilisateurRequest requete = new("Martin", "Alice", "alice@repairit.fr", RoleUtilisateur.Gestionnaire);

    public UtilisateursControllerTests()
    {
        controller = new UtilisateursController(service.Object);
    }

    [Fact]
    public async Task Creer_EmailDejaUtilise_RetourneConflictSansCreer()
    {
        service.Setup(s => s.EmailDejaUtiliseAsync(requete.Email)).ReturnsAsync(true);

        var reponse = await controller.Creer(requete);

        Assert.IsType<ConflictObjectResult>(reponse.Result);
        service.Verify(s => s.CreerAsync(It.IsAny<CreerUtilisateurRequest>()), Times.Never);
    }

    [Fact]
    public async Task Creer_EmailLibre_RetourneCreated()
    {
        service.Setup(s => s.CreerAsync(requete)).ReturnsAsync(new Utilisateur { Id = 6 });

        var reponse = await controller.Creer(requete);

        var created = Assert.IsType<CreatedAtActionResult>(reponse.Result);
        Assert.Equal(6, created.RouteValues!["id"]);
    }

    [Fact]
    public async Task Lister_RetourneOkAvecLesUtilisateurs()
    {
        service.Setup(s => s.ListerAsync()).ReturnsAsync([new Utilisateur { Id = 1 }]);

        var reponse = await controller.Lister();

        Assert.IsType<OkObjectResult>(reponse.Result);
    }

    [Fact]
    public async Task ObtenirParId_UtilisateurInexistant_RetourneNotFound()
    {
        var reponse = await controller.ObtenirParId(404);

        Assert.IsType<NotFoundResult>(reponse.Result);
    }
}
